using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Infrastructure.Persistence.Entities;

public class UserGroup : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string[] Permissions { get; set; }

    [JsonIgnore] public List<User> Users { get; set; }
}