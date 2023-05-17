using FluentValidation;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public static class ImportGrades
{
    public class Command : IRequest
    {
        public Guid UserId { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string JsonEncrypted { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.JsonEncrypted).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);

            if (user is null) throw new NotFoundException(nameof(User), request.UserId);

            var gradeImport = request.Body.Adapt<GradeImport>();

            gradeImport.UserId = user.Id;
            user.ImportAvailable = true;

            await _context.AddAsync(gradeImport, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}