using Blueboard.Core.Auth.Services;
using Blueboard.Features.StudentParites.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Ganss.Xss;
using Mapster;
using MediatR;

namespace Blueboard.Features.StudentParites.Commands;

public static class CreateStudentParty
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string RichTextContent { get; set; }

        public List<ResponsePartyMember> Members { get; set; }

        public string ProgramPlanUrl { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public Guid UserId { get; set; }
    }

    public class ResponsePartyMember
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string? Role { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RichTextContent { get; set; }

        public List<RequestBodyPartyMember> Members { get; set; }

        public string ProgramPlanUrl { get; set; }
        public string VideoUrl { get; set; }
        public string PosterUrl { get; set; }
    }

    public class RequestBodyPartyMember
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string? Role { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
            RuleFor(x => x.RichTextContent).NotEmpty();
            RuleFor(x => x.Members).NotNull();
            RuleForEach(x => x.Members).ChildRules(member =>
            {
                member.RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
                member.RuleFor(x => x.Class).NotEmpty().MaximumLength(255);
                member.RuleFor(x => x.Role).MaximumLength(255);
            });
            RuleFor(x => x.ProgramPlanUrl).NotEmpty();
            RuleFor(x => x.VideoUrl).NotEmpty();
            RuleFor(x => x.PosterUrl).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _publisher;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, IPublisher publisher, UserAccessor userAccessor)
        {
            _context = context;
            _publisher = publisher;
            _userAccessor = userAccessor;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var studentParty = request.Body.Adapt<StudentParty>();

            var htmlSanitizer = new HtmlSanitizer();
            studentParty.RichTextContent = htmlSanitizer.Sanitize(request.Body.RichTextContent);

            studentParty.UserId = _userAccessor.User.Id;

            await _context.StudentParties.AddAsync(studentParty, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new StudentPartiesUpdatedEvent(), cancellationToken);

            return studentParty.Adapt<Response>();
        }
    }
}