using MediatR;
using SharedKernel;
using SupplierOrderService.Application.Dto;

namespace SupplierOrderService.Application.Order.Create;

public sealed record CreateOrderCommand(OrderDto Order) : IRequest<Result<Guid>>;