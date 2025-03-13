using SharedKernel;

namespace CustomerOrderService.Core.Entities.OrderErrors;

public static class OrderErrors
{
    public static readonly Error CreateOrderOnSupplier = Error.Conflict(
        "Order.CreateOrderOnSupplier",
        "Não foi possivel criar a order junto ao fornecedor");
}
