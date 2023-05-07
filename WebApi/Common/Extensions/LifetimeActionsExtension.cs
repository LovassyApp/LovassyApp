using System.Reflection;
using WebApi.Common.Models;
using WebApi.Common.Services.Hosted;

namespace WebApi.Common.Extensions;

public static class LifetimeActionsExtension
{
    /// <summary>
    ///     Adds all lifetime actions in the executing assembly and registers the <see cref="LifetimeActionsService" /> as a
    ///     hosted service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddLifetimeActions(this IServiceCollection services)
    {
        var lifetimeActionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ILifetimeAction)) && t is { IsInterface: false, IsAbstract: false });

        foreach (var lifetimeActionType in lifetimeActionTypes)
            services.AddTransient(typeof(ILifetimeAction), lifetimeActionType);

        services.AddHostedService<LifetimeActionsService>();
    }
}