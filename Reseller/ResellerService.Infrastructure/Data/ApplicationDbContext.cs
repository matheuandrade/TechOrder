using Microsoft.EntityFrameworkCore;
using ResellerService.Aplication.Abstractions.Data;
using ResellerService.Core.Entities.ResellerAggregate;

namespace ResellerService.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Reseller> Resellers { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
