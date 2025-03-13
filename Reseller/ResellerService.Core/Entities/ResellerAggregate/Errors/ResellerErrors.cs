using SharedKernel;

namespace ResellerService.Core.Entities.ResellerAggregate.Errors;

public static class ResellerErrors
{
    public static readonly Error CNPJNotUnique = Error.Conflict(
        "Resellers.CNPJNotUnique",
        "Empresa já cadastrada");

    public static readonly Error CNPJNotUniqueOnSupplier = Error.Conflict(
        "Resellers.CNPJNotUniqueOnSupplier",
        "Empresa já cadastrada junto ao fornecedor");
}
