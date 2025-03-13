using CustomerOrderService.Application.Dto;
using MediatR;
using SharedKernel;

namespace CustomerOrderService.Application.Order.Create;

public sealed record CreateOrderCustomerCommand(OrderDto Order) : IRequest<Result<Guid>>;
