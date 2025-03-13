namespace CustomerOrderService.Core.Entities;

public class Order
{
    public Guid Id { get; set; }

    public string CPF { get; set; } = null!;

    public List<Product> Products { get; set; } = [];
}
