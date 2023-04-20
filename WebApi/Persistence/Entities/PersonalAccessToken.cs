using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Persistence.Entities;

[Index(nameof(Token), IsUnique = true)]
public class PersonalAccessToken : BaseEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [Required] public User User { get; set; }

    [Required] public string Token { get; set; }

    [Required] public DateTime LastUsedAt { get; set; }
}