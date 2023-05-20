using Helpers.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Helpers.Framework.Extensions;

public static class HostExtension
{
    /// <summary>
    ///     Runs the host as normal but if the first argument is a registered console command, it will be executed
    ///     and the execution will stop afterwards.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="args">The commandline args.</param>
    public static void RunWithCommands(this IHost host, string[] args)
    {
        if (args.Length >= 1)
        {
            var commandServices = host.Services.GetServices<IConsoleCommand>();
            var commandIndex = args[0] == "run" ? 1 : 0;
            try
            {
                var consoleCommand = commandServices.FirstOrDefault(t => t.Name == args[commandIndex]);
                if (consoleCommand != null)
                {
                    var commandArgs = new List<string>();
                    for (var i = commandIndex + 1; i < args.Length; i++) commandArgs.Add(args[i]);

                    consoleCommand.Execute(commandArgs.ToArray());
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        host.Run();
    }

    public static async Task RunStartupActions(this IHost host)
    {
        var startupActions = host.Services.GetServices<IStartupAction>();
        var environment = host.Services.GetRequiredService<IHostEnvironment>();
        var logger = host.Services.GetRequiredService<ILogger<IHost>>();
        foreach (var startupAction in startupActions)
            try
            {
                await startupAction.Execute();
            }
            catch
            {
                if (!environment.IsDevelopment())
                    throw;
                logger.LogError($"Error executing startup action {startupAction.GetType().Name}");
            }
    }
}