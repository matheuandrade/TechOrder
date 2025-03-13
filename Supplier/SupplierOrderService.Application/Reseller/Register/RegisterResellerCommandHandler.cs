using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;
using SupplierOrderService.Application.Abstractions.Data;
using SupplierOrderService.Core.Entities.Errors;

namespace SupplierOrderService.Application.Reseller.Register;

public class RegisterResellerCommandHandler(IApplicationDbContext context,
                                            ILogger<RegisterResellerCommandHandler> logger)
    : IRequestHandler<RegisterResellerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterResellerCommand request, CancellationToken cancellationToken)
    {
        if (await context.Resellers.AnyAsync(x => x.CNPJ == request.Reseller.CNPJ, cancellationToken))
        {
            return Result.Failure<Guid>(ResellerErrors.CNPJNotUnique);
        }        

        var newReseller = new Core.Entities.Reseller()
        {
            CNPJ = request.Reseller.CNPJ
        };

        await context.Resellers.AddAsync(newReseller, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Company created with id: {id}", newReseller.Id);

        return newReseller.Id;
    }
}
