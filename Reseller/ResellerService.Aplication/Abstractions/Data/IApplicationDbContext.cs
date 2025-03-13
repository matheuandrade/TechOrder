using Microsoft.EntityFrameworkCore;

namespace ResellerService.Aplication.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Core.Entities.ResellerAggregate.Reseller> Resellers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
