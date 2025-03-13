using CustomerOrderService.Application.Abstractions.Data;
using CustomerOrderService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrderService.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Order> Orders { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
