namespace WebApi.Core.Auth.Interfaces;

public interface IPermission
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
}