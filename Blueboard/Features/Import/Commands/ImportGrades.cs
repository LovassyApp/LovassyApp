using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;

namespace Blueboard.Features.Import.Commands;

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

    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(request.UserId);

            if (user is null) throw new NotFoundException(nameof(User), request.UserId);

            var gradeImport = request.Body.Adapt<GradeImport>();

            gradeImport.UserId = user.Id;
            user.ImportAvailable = true;

            await context.AddAsync(gradeImport, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}