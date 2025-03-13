using MediatR;
using SharedKernel;
using SupplierOrderService.Application.Dtos;

namespace SupplierOrderService.Application.Reseller.Register;

public sealed record RegisterResellerCommand(ResellerDto Reseller) : IRequest<Result<Guid>>;
