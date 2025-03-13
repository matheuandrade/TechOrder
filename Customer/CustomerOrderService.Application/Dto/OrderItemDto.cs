namespace CustomerOrderService.Application.Dto;

public class OrderItemDto
{
    public string ProductReference { get; set; } = null!;

    public int Quantity { get; set; }
}
