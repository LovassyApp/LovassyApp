using Helpers.Email.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers.Email;

public static class AddEmailHelpersExtension
{
    /// <summary>
    ///     Adds all email related helpers to the service collection. This has to be a separate helper because the email views
    ///     have to be in a separate razor template project.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    public static void AddEmailHelpers(this IServiceCollection services, IConfiguration configuration)
    {
        //Fluent Email
        services
            .AddFluentEmail(configuration.GetValue<string>("Email:From"))
            .AddSmtpSender(configuration.GetValue<string>("Email:Host"),
                configuration.GetValue<int>("Email:Port"), configuration.GetValue<string>("Email:Username"),
                configuration.GetValue<string>("Email:Password"));

        services.AddRazorPages();

        services.AddScoped<RazorViewToStringRenderer>();
    }
}