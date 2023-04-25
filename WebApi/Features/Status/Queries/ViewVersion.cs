using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using WebApi.Features.Status.Options;

namespace WebApi.Features.Status.Queries;

public class ViewVersionQuery : IRequest<ViewVersionResponse>
{
    public ViewVersionBody Body { get; set; }
}

public class ViewVersionResponse
{
    public string WhoAmI { get; set; }
    public string Version { get; set; }
    public string DotNetVersion { get; set; }
    public List<string> Contributors { get; set; }
    public string Repository { get; set; }
    public string? MOTD { get; set; }
}

public class ViewVersionBody
{
    public bool SendOk { get; set; } = true;
    public bool SendMOTD { get; set; } = true;
}

public class ViewVersionQueryValidator : AbstractValidator<ViewVersionQuery>
{
    public ViewVersionQueryValidator()
    {
        RuleFor(x => x.Body.SendOk).NotNull();
        RuleFor(x => x.Body.SendMOTD).NotNull();
    }
}

internal sealed class ViewVersionQueryHandler : IRequestHandler<ViewVersionQuery, ViewVersionResponse>
{
    private readonly StatusOptions _options;

    public ViewVersionQueryHandler(IOptions<StatusOptions> options)
    {
        _options = options.Value;
    }

    public async Task<ViewVersionResponse> Handle(ViewVersionQuery request, CancellationToken cancellationToken)
    {
        var response = new ViewVersionResponse
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