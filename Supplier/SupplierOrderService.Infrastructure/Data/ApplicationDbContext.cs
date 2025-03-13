using Microsoft.EntityFrameworkCore;
using SupplierOrderService.Application.Abstractions.Data;
using SupplierOrderService.Core.Entities;

namespace SupplierOrderService.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Reseller> Resellers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Core.Entities.Order> Orders { get; }

    public DbSet<Core.Entities.OrderItem> OrderItems { get; }
}
