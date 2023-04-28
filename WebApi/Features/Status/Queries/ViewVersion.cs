using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using WebApi.Features.Status.Options;

namespace WebApi.Features.Status.Queries;

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

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly StatusOptions _options;

        public Handler(IOptions<StatusOptions> options)
        {
            _options = options.Value;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var response = new Response
            {
                WhoAmI = _options.WhoAmI,
                Version = _options.Version,
                DotNetVersion = Environment.Version.ToString(),
                Contributors = _options.Contributors,
                Repository = _options.Repository,
                MOTD = request.Body.SendMOTD ? _options.MOTDs[new Random().Next(0, _options.MOTDs.Count)] : null
            };

            return response;
        }
    }
}