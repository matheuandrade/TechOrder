using CustomerOrderService.Infrastructure.External.Suppliers.Models;

namespace CustomerOrderService.Infrastructure.External.Suppliers.Interfaces;

public interface ISupplierApiService
{
    Task<Guid?> CreateSupplyOrder(CreateOrderSupplierDto model, CancellationToken cancellationToken);
}
