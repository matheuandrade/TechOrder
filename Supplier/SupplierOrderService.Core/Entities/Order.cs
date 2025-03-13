namespace SupplierOrderService.Core.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ResellerCnpj { get; set; }

    public List<OrderItem> Products { get; set; } = [];

    public Order() { }

    private Order(string cnpj, List<OrderItem> products)
    {
        ResellerCnpj = cnpj;

        products.ForEach(x =>
        {
            x.OrderId = Id;
        });
    }

    public static Order Create(string cnpj, List<OrderItem> products)
    {
        return new Order(cnpj, products);
    }
}
