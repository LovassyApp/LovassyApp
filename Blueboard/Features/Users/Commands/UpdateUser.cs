using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using FluentValidation.Results;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Helpers.WebApi.Exceptions.ValidationException;

namespace Blueboard.Features.Users.Commands;

public static class UpdateUser
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [AdaptIgnore] public int[] UserGroups { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;

        public RequestBodyValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255)
                .Must(EndWithAllowedDomainEmail).WithMessage("Az email cím '@lovassy.edu.hu'-ra kell hogy végződjön.");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.UserGroups).NotNull()
                .MustAsync(BeExistingUserGroups)
                .WithMessage("A megadott felhasználói csoportok közül legalább egy nem létezik.");
        }

        private bool EndWithAllowedDomainEmail(RequestBody model, string email)
        {
            return email.EndsWith("@lovassy.edu.hu");
        }

        private async Task<bool> BeUniqueEmailAsync(RequestBody model, string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users.AllAsync(x => x.Email != email, cancellationToken);
        }

        private async Task<bool> BeExistingUserGroups(RequestBody model, int[] userGroups,
            CancellationToken cancellationToken)
        {
            return await _context.UserGroups.Select(g => g.Id)
                       .CountAsync(id => userGroups.Contains(id), cancellationToken) ==
                   userGroups.Distinct().Count();
        }
    }

    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(u => u.Id == request.Id).Include(u => u.UserGroups)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User), request.Id);

            if (user.Email != request.Body.Email &&
                await context.Users.AnyAsync(x => x.Email == request.Body.Email, cancellationToken))
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(request.Body.Email), "A megadott email cím már foglalt.")
                }); // We have to do this here as the validator is not aware of the user

            request.Body.Adapt(user);

            // Remove groups that are not in the new list
            var groupsToRemove = user.UserGroups
                .Where(g => !request.Body.UserGroups.Contains(g.Id))
                .ToList();

            foreach (var groupToRemove in groupsToRemove) user.UserGroups.Remove(groupToRemove);

            // Add groups that are not in the old list
            var existingGroupIds = new HashSet<int>(user.UserGroups.Select(g => g.Id));

            foreach (var groupId in request.Body.UserGroups)
                if (!existingGroupIds.Contains(groupId))
                {
                    var userGroup = new UserGroup
                    {
                        Id = groupId
                    };
                    context.Entry(userGroup).State = EntityState.Unchanged;
                    user.UserGroups.Add(userGroup);
                }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}