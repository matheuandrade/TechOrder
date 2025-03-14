﻿namespace CustomerOrderService.Application.Dto;

public class OrderDto
{
    //cpf client
    public string CPF { get; set; } = null!;

    public List<OrderItemDto> Products { get; set; } = [];
}
