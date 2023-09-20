using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Entities.Owned;

[Owned]
public class ImageVotingAspect
{
    [Required] public string Key { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }
}