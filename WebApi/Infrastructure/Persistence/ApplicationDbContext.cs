using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ImportKey> ImportKeys { get; set; }
    public DbSet<GradeImport> GradeImports { get; set; }
    public DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Lolo> Lolos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasPostgresEnum<GradeType>();
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTimestamps();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is TimestampedEntity && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((TimestampedEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
                ((TimestampedEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
        }
    }
}