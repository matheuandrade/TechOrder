namespace SupplierOrderService.Core.Entities;

public class Reseller
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CNPJ { get; set; } = null!;
}
