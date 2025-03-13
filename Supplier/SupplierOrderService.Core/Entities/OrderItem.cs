namespace SupplierOrderService.Core.Entities;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string ProductReference { get; set; } = null!;

    public int Quantity { get; set; }
}
