using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blueboard.Infrastructure.Persistence.Entities;

[Index(nameof(Token), IsUnique = true)]
public class PersonalAccessToken : TimestampedEntity
{
    [Key] public int Id { get; set; }

    [Required] public Guid UserId { get; set; }
    [JsonIgnore] public User User { get; set; }

    [Required] public string Token { get; set; }

    public DateTime? LastUsedAt { get; set; }
}

public class PersonalAccessTokenConfiguration : IEntityTypeConfiguration<PersonalAccessToken>
{
    public void Configure(EntityTypeBuilder<PersonalAccessToken> builder)
    {
        builder.HasOne(i => i.User).WithMany(u => u.PersonalAccessTokens).HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}