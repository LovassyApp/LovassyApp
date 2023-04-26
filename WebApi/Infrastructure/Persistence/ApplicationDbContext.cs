using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.Common.Services;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly DomainEventService _domainEventService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DomainEventService domainEventService) :
        base(options)
    {
        _domainEventService = domainEventService;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ImportKey> ImportKeys { get; set; }
    public DbSet<GradeImport> GradeImports { get; set; }
    public DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        var result = base.SaveChanges();
        DispatchEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTimestamps();
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchEvents();
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

    private async Task DispatchEvents()
    {
        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();


        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}