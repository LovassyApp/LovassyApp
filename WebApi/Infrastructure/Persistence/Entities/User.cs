using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Common;

namespace WebApi.Infrastructure.Persistence.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(MasterKeySalt), IsUnique = true)]
[Index(nameof(OmCodeHashed), IsUnique = true)]
[Index(nameof(HasherSaltHashed), IsUnique = true)]
public class User : TimestampedEntity, IHasDomainEvent
{
    [Key] public Guid Id { get; set; }

    [Required] [MaxLength(255)] public string Name { get; set; }
    [Required] public string Email { get; set; }
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

    public string? RealName { get; set; }
    public string? Class { get; set; }

    public bool ImportAvailable { get; set; }

    public List<GradeImport> GradeImports { get; set; }
    public List<Grade> Grades { get; set; }
    public List<Lolo> Lolos { get; set; }

    public List<DomainEvent> DomainEvents { get; } = new();
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.ImportAvailable).HasDefaultValue(false);
        builder.Ignore(u => u.DomainEvents);
    }
}