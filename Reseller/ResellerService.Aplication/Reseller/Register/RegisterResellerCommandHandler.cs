using MediatR;
using Microsoft.Extensions.Logging;
using ResellerService.Core.Entities;
using ResellerService.Core.Entities.ResellerAggregate;
using ResellerService.Core.Entities.ResellerAggregate.Errors;
using ResellerService.Core.External.Supplier;
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
        logger.LogInformation("Starting to register reseller with CNPJ: {CNPJ}", request.Reseller.CNPJ);

        // Verificando se o CNPJ já existe no banco de dados
        if (await repository.AnyAsync(x => x.CNPJ == request.Reseller.CNPJ, cancellationToken))
        {
            logger.LogWarning("Registration failed for reseller with CNPJ: {CNPJ}. CNPJ is not unique.", request.Reseller.CNPJ);
            return Result.Failure<Guid>(ResellerErrors.CNPJNotUnique);
        }

        // Enviando o pedido para criar o fornecedor
        logger.LogInformation("Creating supplier with CNPJ: {CNPJ} on external API", request.Reseller.CNPJ);

        var createSupplierResponse = await supplierApiService.CreateSupplier(request.Reseller.CNPJ, cancellationToken);

        if (createSupplierResponse is null)
        {
            logger.LogError("Supplier creation failed for CNPJ: {CNPJ}. Supplier response was null.", request.Reseller.CNPJ);
            return Result.Failure<Guid>(ResellerErrors.CNPJNotUniqueOnSupplier);
        }

        // Logando sucesso na criação do fornecedor
        logger.LogInformation("Supplier created with id: {SupplierId} for reseller with CNPJ: {CNPJ}", createSupplierResponse, request.Reseller.CNPJ);

        // Criando a entidade Reseller para salvar no banco de dados
        var newReseller = Core.Entities.ResellerAggregate.Reseller.Create(
            request.Reseller.CNPJ,
            request.Reseller.CorporateName,
            request.Reseller.TradeName,
            request.Reseller.Email, // Evitar logar emails diretamente se não for seguro
            request.Reseller.Phones.Select(p => Phone.Create(p.PhoneAddres)).ToList(),
            request.Reseller.Contacts.Select(c => Contacts.Create(c.Name, c.IsPrimary)).ToList(),
            request.Reseller.Addresses.Select(a => Address.Create(a.StreetAddress, a.Number, a.ZipCode, a.Complement, a.City, a.State)).ToList()
        );

        logger.LogInformation("Adding new reseller with CNPJ: {CNPJ} to the database", request.Reseller.CNPJ);

        // Persistindo o novo reseller no banco de dados
        await repository.AddAsync(newReseller, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully registered reseller with ID: {ResellerId} and CNPJ: {CNPJ}", newReseller.Id, request.Reseller.CNPJ);

        return newReseller.Id;
    }
}
