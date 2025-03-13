using SharedKernel;

namespace SupplierOrderService.Core.Entities.Errors;

public static class OrderErrors
{
    public static readonly Error CNPJNotUnique = Error.Conflict(
        "Resellers.CNPJNotUnique",
        "Empresa já cadastrada");
}
