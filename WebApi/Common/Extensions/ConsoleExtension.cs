using System.Reflection;
using WebApi.Common.Models;

namespace WebApi.Common.Extensions;

public static class ConsoleExtension
{
    /// <summary>
    ///     Adds all console commands in the executing assembly to services. Console command have to inherit from
    ///     <see cref="IConsoleCommand" />.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddConsoleCommands(this IServiceCollection services)
    {
        var commandTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IConsoleCommand)) && t is { IsInterface: false, IsAbstract: false });

        foreach (var commandType in commandTypes) services.AddTransient(typeof(IConsoleCommand), commandType);
    }

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
}