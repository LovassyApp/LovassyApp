namespace WebApi.Common.Models;

/// <summary>
///     The base interface for all console commands. They are registered in the DI container automatically as transient.
/// </summary>
public interface IConsoleCommand
{
    public string Name { get; }
    public void Execute(string[] args);
}