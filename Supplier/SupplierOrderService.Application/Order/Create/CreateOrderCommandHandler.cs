using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;
using SupplierOrderService.Application.Abstractions.Data;
using SupplierOrderService.Core.Entities;
using SupplierOrderService.Core.Entities.Errors;

namespace SupplierOrderService.Application.Order.Create;

public class CreateOrderCommandHandler(IApplicationDbContext context,
                                            ILogger<CreateOrderCommandHandler> logger)
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Logando a tentativa de criação de um pedido com o CNPJ do revendedor
        logger.LogInformation("Attempting to create order for reseller with CNPJ: {CNPJ}", request.Order.CNPJ);

        // Verificando se o CNPJ existe na base de dados
        if (!await context.Resellers.AnyAsync(x => x.CNPJ == request.Order.CNPJ, cancellationToken))
        {
            logger.LogWarning("Order creation failed: Reseller with CNPJ {CNPJ} not found", request.Order.CNPJ);
            return Result.Failure<Guid>(ResellerErrors.CNPJNotExists);
        }

        // Logando que o revendedor foi encontrado e que o pedido será criado
        logger.LogInformation("Reseller with CNPJ {CNPJ} found. Creating order...", request.Order.CNPJ);

        // Criando o pedido
        var order = Core.Entities.Order.Create(request.Order.CNPJ, request.Order.Products.Select(p =>
        {
            return new OrderItem()
            {
                ProductReference = p.ProductReference,
                Quantity = p.Quantity,
            };
        }).ToList());

        // Logando a criação do pedido
        logger.LogInformation("Created order for reseller with CNPJ: {CNPJ}. Order contains {ProductCount} products.",
                              request.Order.CNPJ, order.Products.Count);

        // Adicionar a validação da quantidade de produtos no inventário do fornecedor
        // Adicione aqui um log para validar a quantidade do produto (caso seja necessário)
        logger.LogInformation("Validating product quantities for the order. Total products: {ProductCount}", order.Products.Count);

        // Todo: adicionar lógica para validar a disponibilidade de estoque no fornecedor

        // Persistindo o pedido no banco de dados
        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // Logando o sucesso na criação do pedido
        logger.LogInformation("Order successfully created with id: {OrderId}", order.Id);

        return order.Id;
    }
}
