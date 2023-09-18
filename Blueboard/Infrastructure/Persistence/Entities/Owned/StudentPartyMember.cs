using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Entities.Owned;

[Owned]
public class StudentPartyMember
{
    [Required] public string Name { get; set; }

    [Required] public string Class { get; set; }

    public string? Role { get; set; }
}