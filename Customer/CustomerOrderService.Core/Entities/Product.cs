namespace CustomerOrderService.Core.Entities;

public class Product
{
    public string ProductName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ProductReference { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
