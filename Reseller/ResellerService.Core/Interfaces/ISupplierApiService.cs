namespace ResellerService.Core.Interfaces;

public interface ISupplierApiService
{
    Task<Guid?> CreateSupplier(string cnpj, CancellationToken cancellationToken);
}
