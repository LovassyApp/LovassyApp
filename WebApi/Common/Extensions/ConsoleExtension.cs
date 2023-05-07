using System.Reflection;
using WebApi.Common.Models;

namespace WebApi.Common.Extensions;

public static class ConsoleExtension
{
    public static IServiceCollection AddConsoleCommands(this IServiceCollection services)
    {
        var commandTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IConsoleCommand)) && t is { IsInterface: false, IsAbstract: false });

        foreach (var commandType in commandTypes) services.AddTransient(typeof(IConsoleCommand), commandType);

        return services;
    }

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