namespace WebApi.Common.Models;

public interface IConsoleCommand
{
    public string Name { get; }
    public void Execute(string[] args);
}