namespace WebApi.Contexts.Users.Models;

public class CreateUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }

    public string Name { get; set; }
    public string OmCode { get; set; }
}