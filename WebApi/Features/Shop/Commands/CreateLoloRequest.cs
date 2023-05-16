using FluentValidation;
using Mapster;
using MediatR;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Commands;

public static class CreateLoloRequest
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(65535);
        }
    }

    public class Response
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public Guid UserId { get; set; }

        public DateTime? AcceptedAt { get; set; }
        public DateTime? DeniedAt { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAccessor _userAccessor;

        public Handler(UserAccessor userAccessor, ApplicationDbContext context)
        {
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = request.Body.Adapt<LoloRequest>();
            loloRequest.UserId = _userAccessor.User!.Id;

            await _context.LoloRequests.AddAsync(loloRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            //TODO: Send out an event and when handling it send a notification through websockets and maybe even an email to users who can handle it

            return loloRequest.Adapt<Response>();
        }
    }
}