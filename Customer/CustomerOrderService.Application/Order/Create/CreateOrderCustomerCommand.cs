using CustomerOrderService.Application.Dto;
using MediatR;
using SharedKernel;

namespace CustomerOrderService.Application.Order.Create;

public sealed record CreateOrderCustomerCommand(string CNPJ, OrderDto Order) : IRequest<Result<Guid>>;
