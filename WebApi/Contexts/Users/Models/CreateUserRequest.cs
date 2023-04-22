using System.ComponentModel.DataAnnotations;

namespace WebApi.Contexts.Users.Models;

public class CreateUserRequest
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Password { get; set; }

    [Required] public string Name { get; set; }
    [Required] public string OmCode { get; set; }
}