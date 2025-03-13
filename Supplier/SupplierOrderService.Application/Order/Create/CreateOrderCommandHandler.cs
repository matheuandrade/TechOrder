using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;
using SupplierOrderService.Application.Abstractions.Data;
using SupplierOrderService.Core.Entities;
using SupplierOrderService.Core.Entities.Errors;

namespace SupplierOrderService.Application.Order.Create;

public class RegisterResellerCommandHandler(IApplicationDbContext context,
                                            ILogger<RegisterResellerCommandHandler> logger)
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (!await context.Resellers.AnyAsync(x => x.CNPJ == request.Order.CNPJ, cancellationToken))
        {
            return Result.Failure<Guid>(ResellerErrors.CNPJNotExists);
        }

        var order = Core.Entities.Order.Create(request.Order.CNPJ, request.Order.Products.Select(p =>
        {
            return new OrderItem()
            {
                ProductReference = p.ProductReference,
                Quantity = p.Quantity,
            };
        }).ToList());

        //Todo: add some logic to validate the product quantity is available on supplier inventory

        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order created with id: {Id}", order.Id);

        return order.Id;
    }
}
