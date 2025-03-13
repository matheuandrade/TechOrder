using MediatR;
using ResellerService.Aplication.Dtos;
using SharedKernel;

namespace ResellerService.Aplication.Reseller.Register;

public sealed record RegisterResellerCommand(ResellerDto Reseller) : IRequest<Result<Guid>>;
