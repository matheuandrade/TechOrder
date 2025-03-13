using CustomerOrderService.Core.Entities.OrderErrors;
using CustomerOrderService.Core.External.Suppliers.Interfaces;
using CustomerOrderService.Core.External.Suppliers.Models;
using CustomerOrderService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace CustomerOrderService.Application.Order.Create;

public class CreateOrderCustomerCommandHandler(IRepository<Core.Entities.Order> repository,
                                            ISupplierApiService supplierApiService,
                                            ILogger<CreateOrderCustomerCommand> logger)
    : IRequestHandler<CreateOrderCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCustomerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to create order for customer with CPF: {CPF}, CNPJ: {CNPJ}", request.Order.CPF, request.CNPJ);

        // Preparando o DTO para o pedido do fornecedor
        var orderSupplierRequest = new CreateOrderSupplierDto
        {
            CNPJ = request.CNPJ,
            Products = request.Order.Products
                .Select(x => new OrderItemDto
                {
                    ProductReference = x.ProductReference,
                    Quantity = x.Quantity
                })
                .ToList()
        };

        // Logando a requisição antes de chamar a API externa
        logger.LogInformation("Sending order to supplier with CNPJ: {CNPJ} and {ProductCount} products", request.CNPJ, request.Order.Products.Count);

        try
        {
            var createOrderSupplierResponse = await supplierApiService.CreateSupplyOrder(orderSupplierRequest, cancellationToken);

            if (createOrderSupplierResponse is null)
            {
                // Logando falha de resposta nula da API
                logger.LogError("Failed to create order on supplier for CNPJ: {CNPJ}. Supplier response was null.", request.CNPJ);
                return Result.Failure<Guid>(OrderErrors.CreateOrderOnSupplier);
            }

            // Logando a resposta bem-sucedida da API
            logger.LogInformation("Company created on supplier with id: {SupplierOrderId} for CNPJ: {CNPJ}", createOrderSupplierResponse, request.CNPJ);

            // Preparando a entidade de ordem para persistência
            var order = new Core.Entities.Order()
            {
                CPF = request.Order.CPF,
                Products = request.Order.Products
                    .Select(x => new Core.Entities.OrderItem
                    {
                        ProductReference = x.ProductReference,
                        Quantity = x.Quantity
                    })
                    .ToList()
            };

            // Logando a inserção do pedido no banco de dados
            logger.LogInformation("Saving order for CPF: {CPF} with {ProductCount} items to the database", request.Order.CPF, order.Products.Count);

            await repository.AddAsync(order, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            // Logando a conclusão com sucesso da operação
            logger.LogInformation("Order successfully created with id: {OrderId} for CPF: {CPF}", order.Id, request.Order.CPF);

            return order.Id;
        }
        catch (Exception ex)
        {
            // Logando exceções inesperadas
            logger.LogError(ex, "An error occurred while processing the order for CNPJ: {CNPJ}, CPF: {CPF}", request.CNPJ, request.Order.CPF);
            return Result.Failure<Guid>(OrderErrors.InternalError);
        }
    }
}
