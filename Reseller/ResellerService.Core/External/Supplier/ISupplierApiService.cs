namespace ResellerService.Core.External.Supplier;

public interface ISupplierApiService
{
    Task<Guid?> CreateSupplier(string cnpj, CancellationToken cancellationToken);
}
