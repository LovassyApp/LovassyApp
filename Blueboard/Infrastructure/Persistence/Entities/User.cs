using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Blueboard.Infrastructure.Persistence.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(MasterKeySalt), IsUnique = true)]
[Index(nameof(OmCodeHashed), IsUnique = true)]
[Index(nameof(HasherSaltHashed), IsUnique = true)]
public class User : TimestampedEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Email { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    [Required] public string PasswordHashed { get; set; }

    [Required] public string PublicKey { get; set; }
    [Required] public string PrivateKeyEncrypted { get; set; }
    [Required] public string MasterKeyEncrypted { get; set; }
    [Required] public string MasterKeySalt { get; set; }
    [Required] public string ResetKeyEncrypted { get; set; }
    [Required] public string HasherSaltEncrypted { get; set; }
    [Required] public string HasherSaltHashed { get; set; }

    [Required] public string OmCodeEncrypted { get; set; }
    [Required] public string OmCodeHashed { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? RealName { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? Class { get; set; }

    public bool ImportAvailable { get; set; }

    [JsonIgnore] public List<UserGroup> UserGroups { get; set; }

    [JsonIgnore] public List<GradeImport> GradeImports { get; set; }

    [JsonIgnore] public List<Lolo> Lolos { get; set; }

    [JsonIgnore] public List<LoloRequest> LoloRequests { get; set; }

    [JsonIgnore] public List<StoreHistory> StoreHistories { get; set; }

    [JsonIgnore] public List<OwnedItem> OwnedItems { get; set; }

    [JsonIgnore] public List<PersonalAccessToken> PersonalAccessTokens { get; set; }

    [JsonIgnore] public List<StudentParty> StudentParties { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.ImportAvailable).HasDefaultValue(false);
    }
}