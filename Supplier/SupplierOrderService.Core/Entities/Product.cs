namespace SupplierOrderService.Core.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ProductReference { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
