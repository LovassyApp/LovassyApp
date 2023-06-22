using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Entities;

[Index(nameof(Email), IsUnique = true)]
public class ResetKeyPasswordSetNotifier : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public string Email { get; set; }
}