using Microsoft.EntityFrameworkCore;

namespace CustomerOrderService.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Core.Entities.Order> Orders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
