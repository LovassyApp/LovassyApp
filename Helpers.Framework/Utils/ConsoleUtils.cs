namespace Helpers.Framework.Utils;

/// <summary>
///     Utility class for logging to the console. Primarily meant for use in console commands and not for logging!
/// </summary>
public static class ConsoleUtils
{
    /// <summary>
    ///     Outputs a message to the console with a green color.
    /// </summary>
    /// <param name="message">The message string to output.</param>
    public static void Success(string message)
    {
        LogMessage(message, ConsoleColor.Green);
    }

    /// <summary>
    ///     Outputs a message to the console with a red color.
    /// </summary>
    /// <param name="message">The message string to output.</param>
    public static void Error(string message)
    {
        LogMessage(message, ConsoleColor.Red);
    }

    /// <summary>
    ///     Outputs a message to the console with a blue color.
    /// </summary>
    /// <param name="message">The message string to output.</param>
    public static void Info(string message)
    {
        LogMessage(message, ConsoleColor.Blue);
    }

    /// <summary>
    ///     Outputs a message to the console with a yellow color.
    /// </summary>
    /// <param name="message">The message string to output.</param>
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