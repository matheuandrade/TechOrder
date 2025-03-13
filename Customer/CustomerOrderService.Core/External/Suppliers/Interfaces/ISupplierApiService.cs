using CustomerOrderService.Core.External.Suppliers.Models;

namespace CustomerOrderService.Core.External.Suppliers.Interfaces;

public interface ISupplierApiService
{
    Task<Guid?> CreateSupplyOrder(CreateOrderSupplierDto model, CancellationToken cancellationToken);
}
