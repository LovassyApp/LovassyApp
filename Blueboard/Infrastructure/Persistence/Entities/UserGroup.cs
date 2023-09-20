using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

public class UserGroup : TimestampedEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    [Key]
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    [Required]
    public string Name { get; set; }

    [Required] public string[] Permissions { get; set; }

    [JsonIgnore] public List<User> Users { get; set; }

    [JsonIgnore] public List<ImageVoting> ImageVotings { get; set; }
}