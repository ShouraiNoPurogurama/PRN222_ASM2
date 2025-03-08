using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SalesManagement.Repository.Abstractions;

namespace SalesManagement.Repository.Data.Interceptors;

public class AuditableEntityInterceptor(IHttpContextAccessor contextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new ())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? dbContext)
    {
        if (dbContext is null) return;

        foreach (EntityEntry<IEntity> entry in dbContext.ChangeTracker.Entries<IEntity>()) //this will give us all the entity entries that implement IEntity
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = contextAccessor.HttpContext.User.Identity!.Name!;
                entry.Entity.CreatedAt = DateTimeOffset.Now;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = contextAccessor.HttpContext.User.Identity!.Name!;
                entry.Entity.LastModified = DateTimeOffset.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => r.TargetEntry != null &&
                                  r.TargetEntry.Metadata.IsOwned() &&
                                  r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}