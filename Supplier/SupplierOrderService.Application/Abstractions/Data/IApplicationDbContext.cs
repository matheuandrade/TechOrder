using Microsoft.EntityFrameworkCore;

namespace SupplierOrderService.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Core.Entities.Reseller> Resellers { get; }

    DbSet<Core.Entities.Product> Products { get; }

    DbSet<Core.Entities.Order> Orders { get; }

    DbSet<Core.Entities.OrderItem> OrderItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
