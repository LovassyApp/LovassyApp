using FluentValidation;
using Mapster;
using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public class ImportGradesCommand : IRequest
{
    public Guid UserId { get; set; }
    public ImportGradesBody Body { get; set; }
}

public class ImportGradesBody
{
    public string KeyEncrypeted { get; set; }
    public string JsonEncrypted { get; set; }
}

public class ImportGradesBodyValidator : AbstractValidator<ImportGradesBody>
{
    public ImportGradesBodyValidator()
    {
        RuleFor(x => x.KeyEncrypeted).NotEmpty();
        RuleFor(x => x.JsonEncrypted).NotEmpty();
    }
}

internal sealed class ImportGradesCommandHandler : IRequestHandler<ImportGradesCommand>
{
    private readonly ApplicationDbContext _context;

    public ImportGradesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ImportGradesCommand request, CancellationToken cancellationToken)
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