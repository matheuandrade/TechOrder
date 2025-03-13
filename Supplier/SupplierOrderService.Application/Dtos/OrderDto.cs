namespace SupplierOrderService.Application.Dto;

public class OrderDto
{
    //CNPJ revenda
    public string CNPJ { get; set; } = null!;

    public List<ProductDto> Products { get; set; } = [];
}
