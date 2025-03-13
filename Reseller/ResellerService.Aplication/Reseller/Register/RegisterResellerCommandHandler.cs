using MediatR;
using Microsoft.Extensions.Logging;
using ResellerService.Core.Entities;
using ResellerService.Core.Entities.ResellerAggregate;
using ResellerService.Core.Entities.ResellerAggregate.Errors;
using ResellerService.Core.Interfaces;
using SharedKernel;

namespace ResellerService.Aplication.Reseller.Register;

public class RegisterResellerCommandHandler(IRepository<Core.Entities.ResellerAggregate.Reseller> repository,
                                            ISupplierApiService supplierApiService,
                                            ILogger<RegisterResellerCommandHandler> logger)
    : IRequestHandler<RegisterResellerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterResellerCommand request, CancellationToken cancellationToken)
    {
        if (await repository.AnyAsync(x => x.CNPJ == request.Reseller.CNPJ, cancellationToken))
        {
            return Result.Failure<Guid>(ResellerErrors.CNPJNotUnique);
        }

        var createSupplierResponse = await supplierApiService.CreateSupplier(request.Reseller.CNPJ, cancellationToken);

        if (createSupplierResponse is null)
        {
            return Result.Failure<Guid>(ResellerErrors.CNPJNotUniqueOnSupplier);
        }

        logger.LogInformation("Company created on supplier with id: {id}", createSupplierResponse);

        var newReseller = Core.Entities.ResellerAggregate.Reseller.Create(request.Reseller.CNPJ,
                request.Reseller.CorporateName,
                request.Reseller.TradeName,
                request.Reseller.Email,
                request.Reseller.Phones.Select(p => Phone.Create(p.PhoneAddres)).ToList(),  
                request.Reseller.Contacts.Select(c => Contacts.Create(c.Name, c.IsPrimary)).ToList(),
                request.Reseller.Addresses.Select(a => Address.Create(a.StreetAddress, a.Number, a.ZipCode, a.Complement, a.City, a.State)).ToList());

        await repository.AddAsync(newReseller, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return newReseller.Id;
    }
}
