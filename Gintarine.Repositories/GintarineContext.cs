using Gintarine.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gintarine.Repositories;

public class GintarineContext : DbContext
{
    private readonly string[] _excludedProperties =
    {
        nameof(Auditable.CreatedAt),
        nameof(Auditable.Id),
        nameof(Auditable.ModifiedAt)
    };

    private readonly TimeProvider _timeProvider;

    public GintarineContext(DbContextOptions<GintarineContext> options, TimeProvider timeProvider)
        : base(options)
    {
        _timeProvider = timeProvider;
    }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Log> Logs { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AuditEntities()
    {
        foreach (var entityEntry in ChangeTracker.Entries<Auditable>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = _timeProvider.GetUtcNow().DateTime;
                AddLog(entityEntry, null);
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Entity.ModifiedAt = _timeProvider.GetUtcNow().DateTime;
                LogModifiedProperties(entityEntry);
            }
        }
    }

    private void LogModifiedProperties<T>(EntityEntry<T> entityEntry)
        where T : Auditable
    {
        var modifiedProperties = GetModifiedPropertyEntries(entityEntry);

        foreach (var modifiedProperty in modifiedProperties)
        {
            AddLog(entityEntry, modifiedProperty);
        }
    }

    private IEnumerable<PropertyEntry> GetModifiedPropertyEntries(EntityEntry entityEntry)
    {
        var propertyNames = entityEntry.Metadata
            .GetProperties()
            .Select(p => p.Name)
            .ToList();

        return propertyNames
            .Select(entityEntry.Property)
            .Where(e => e.IsModified &&
                        e.CurrentValue != e.OriginalValue &&
                        !_excludedProperties.Contains(e.Metadata.Name));
    }

    private void AddLog<T>(EntityEntry<T> entityEntry, PropertyEntry propertyEntry)
        where T : Auditable
    {
        entityEntry.Entity.Logs ??= new List<Log>();
        var log = GetLogEntity(entityEntry.State, propertyEntry);
        entityEntry.Entity.Logs.Add(log);
    }

    private Log GetLogEntity(EntityState state, PropertyEntry propertyEntry)
    {
        var log = new Log
        {
            Action = state.ToString(),
            Date = _timeProvider.GetUtcNow().DateTime
        };

        if (propertyEntry != null)
        {
            log.NewValue = propertyEntry.CurrentValue?.ToString();
            log.OldValue = propertyEntry.OriginalValue?.ToString();
            log.PropertyName = propertyEntry.Metadata.Name;
        }

        return log;
    }
}