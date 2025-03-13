namespace CustomerOrderService.Core.Entities;

public class OrderItem
{
    public OrderItem() { }

    public Guid OrderId { get; set; } = Guid.NewGuid();

    public string ProductReference { get; set; } = null!;

    public int Quantity { get; set; }
}
