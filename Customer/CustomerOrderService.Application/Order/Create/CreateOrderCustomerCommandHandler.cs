using CustomerOrderService.Core.Entities.OrderErrors;
using CustomerOrderService.Core.Interfaces;
using CustomerOrderService.Infrastructure.External.Suppliers.Interfaces;
using CustomerOrderService.Infrastructure.External.Suppliers.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace CustomerOrderService.Application.Order.Create;

public class CreateOrderCustomerCommandHandler(IRepository<Core.Entities.Order> repository,
                                            ISupplierApiService supplierApiService,
                                            ILogger<CreateOrderCustomerCommand> logger,
                                            IConfiguration config)
    : IRequestHandler<CreateOrderCustomerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCustomerCommand request, CancellationToken cancellationToken)
    {
        var resellerCPNJ = config.GetValue<string>("Revenda:CNPJ")!;

        var orderSupplierRequest = new CreateOrderSupplierDto
        {
            CNPJ = resellerCPNJ,
            Products = request.Order.Products
            .Select(x => new OrderItemDto
            {
                ProductReference = x.ProductReference,
                Quantity = x.Quantity
            })
            .ToList()
        };

        var createOrderSupplierResponse = await supplierApiService.CreateSupplyOrder(orderSupplierRequest, cancellationToken);

        if (createOrderSupplierResponse is null)
        {
            return Result.Failure<Guid>(OrderErrors.CreateOrderOnSupplier);
        }

        logger.LogInformation("Company created on supplier with id: {id}", createOrderSupplierResponse);

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

        await repository.AddAsync(order, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
