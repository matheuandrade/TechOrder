using SharedKernel;

namespace CustomerOrderService.Core.Entities.OrderErrors;

public static class OrderErrors
{
    public static readonly Error CreateOrderOnSupplier = Error.Conflict(
        "Order.CreateOrderOnSupplier",
        "Não foi possivel criar a order junto ao fornecedor");

    public static readonly Error InternalError = Error.Conflict(
        "Order.InternalError",
        "Erro não esperado, contate o administrador");
}
