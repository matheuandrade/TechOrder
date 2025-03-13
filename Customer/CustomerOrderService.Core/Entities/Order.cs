namespace CustomerOrderService.Core.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CPF { get; set; } = null!;

    public List<OrderItem> Products { get; set; } = [];
}
