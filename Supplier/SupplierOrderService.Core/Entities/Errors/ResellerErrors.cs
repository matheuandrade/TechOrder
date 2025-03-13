using SharedKernel;

namespace SupplierOrderService.Core.Entities.Errors;

public static class ResellerErrors
{
    public static readonly Error CNPJNotUnique = Error.Conflict(
        "Resellers.CNPJNotUnique",
        "Empresa já cadastrada");

    public static readonly Error CNPJNotExists = Error.Conflict(
        "Resellers.CNPJNotExists",
        "Empresa Não cadastrada");
}
