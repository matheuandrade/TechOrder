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
        // Logando a tentativa de registrar um novo revendedor com o CNPJ
        logger.LogInformation("Attempting to register reseller with CNPJ: {CNPJ}", request.Reseller.CNPJ);

        // Verificando se já existe um revendedor com o mesmo CNPJ
        if (await context.Resellers.AnyAsync(x => x.CNPJ == request.Reseller.CNPJ, cancellationToken))
        {
            // Logando uma tentativa de registro com falha devido ao CNPJ já existente
            logger.LogWarning("Registration failed: Reseller with CNPJ {CNPJ} already exists", request.Reseller.CNPJ);
            return Result.Failure<Guid>(ResellerErrors.CNPJNotUnique);
        }

        // Logando que o revendedor não existe e o processo de criação será iniciado
        logger.LogInformation("No existing reseller found with CNPJ {CNPJ}. Creating new reseller...", request.Reseller.CNPJ);

        // Criando o novo revendedor
        var newReseller = new Core.Entities.Reseller()
        {
            CNPJ = request.Reseller.CNPJ
        };

        // Logando o início do processo de persistência no banco de dados
        logger.LogInformation("Persisting new reseller with CNPJ: {CNPJ}", request.Reseller.CNPJ);

        // Adicionando o novo revendedor ao contexto e salvando no banco de dados
        await context.Resellers.AddAsync(newReseller, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // Logando o sucesso da criação do revendedor com o ID gerado
        logger.LogInformation("New reseller created successfully with ID: {ResellerId} and CNPJ: {CNPJ}", newReseller.Id, request.Reseller.CNPJ);

        return newReseller.Id;
    }
}
