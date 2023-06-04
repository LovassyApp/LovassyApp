namespace Helpers.WebApi.Interfaces;

/// <summary>
///     The base interface for all actions that should be executed on application start before the first request.
/// </summary>
public interface IStartupAction
{
    public Task Execute();
}