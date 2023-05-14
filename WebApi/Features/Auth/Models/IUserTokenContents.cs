namespace WebApi.Features.Auth.Models;

public interface IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}