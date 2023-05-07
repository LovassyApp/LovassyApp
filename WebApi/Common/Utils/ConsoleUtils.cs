namespace WebApi.Common.Utils;

public static class ConsoleUtils
{
    public static void Success(string message)
    {
        LogMessage(message, ConsoleColor.Green);
    }

    public static void Error(string message)
    {
        LogMessage(message, ConsoleColor.Red);
    }

    public static void Info(string message)
    {
        LogMessage(message, ConsoleColor.Blue);
    }

    public static void Warning(string message)
    {
        LogMessage(message, ConsoleColor.DarkYellow);
    }

    private static void LogMessage(string message, ConsoleColor color)
    {
        var currentForeground = Console.BackgroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = currentForeground;
    }
}