using Blueboard.Features.Status.Services.Options;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace Blueboard.Features.Status.Queries;

public static class ViewVersion
{
    public class Query : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public string WhoAmI { get; set; }
        public string Version { get; set; }
        public string DotNetVersion { get; set; }
        public List<string> Contributors { get; set; }
        public string Repository { get; set; }
        public string? MOTD { get; set; }
    }

    public class RequestBody
    {
        public bool SendOk { get; set; } = true;
        public bool SendMOTD { get; set; } = true;
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.SendOk).NotNull();
            RuleFor(x => x.SendMOTD).NotNull();
        }
    }

    internal sealed class Handler(IOptions<StatusOptions> statusOptions) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var response = new Response
            {
                WhoAmI = statusOptions.Value.WhoAmI,
                Version = statusOptions.Value.Version,
                DotNetVersion = Environment.Version.ToString(),
                Contributors = statusOptions.Value.Contributors,
                Repository = statusOptions.Value.Repository,
                MOTD = request.Body.SendMOTD
                    ? statusOptions.Value.MOTDs[new Random().Next(0, statusOptions.Value.MOTDs.Count)]
                    : null
            };

            return response;
        }
    }
}