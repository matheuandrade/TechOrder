namespace CustomerOrderService.Core.External.Suppliers.Models;

public class CreateOrderSupplierDto
{
    public string CNPJ { get; set; } = null!;

    public List<OrderItemDto> Products { get; set; } = [];
}

public class OrderItemDto
{
    public string ProductReference { get; set; } = null!;

    public int Quantity { get; set; }
}
