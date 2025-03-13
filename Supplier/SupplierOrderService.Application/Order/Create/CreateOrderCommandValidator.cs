using FluentValidation;
using SupplierOrderService.Application.Dto;

namespace SupplierOrderService.Application.Order.Create;

internal sealed class CreateOrderCommandValidator : AbstractValidator<OrderDto>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.CNPJ)
            .NotEmpty().NotNull().WithMessage("CNP é obrigatorio.")
            .Length(14).WithMessage("CNPJ deve conter 14 digitos.")
            .Matches(@"^\d+$").WithMessage("CNPJ deve conter somente números.")
            .Must(IsValidCNPJ).WithMessage("CNPJ inválido.");

        RuleForEach(c => c.Products)
            .ChildRules(products =>
            {
                products.RuleFor(a => a.ProductReference)
                    .NotEmpty().NotNull().WithMessage("Referencia do produto não pode ser nula");

                products.RuleFor(a => a.Quantity)
                    .GreaterThan(0).WithMessage("Quantidade precisa ser maior que zero");
            });
    }

    private static bool IsValidCNPJ(string cnpj)
    {
        if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
            return false;

        int[] firstMultiplier = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] secondMultiplier = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        string cnpjWithoutDigits = cnpj[..12];
        string checkDigits = cnpj.Substring(12, 2);

        int firstCheckDigit = CalculateDigit(cnpjWithoutDigits, firstMultiplier);
        int secondCheckDigit = CalculateDigit(cnpjWithoutDigits + firstCheckDigit, secondMultiplier);

        return checkDigits == $"{firstCheckDigit}{secondCheckDigit}";
    }

    private static int CalculateDigit(string value, int[] multipliers)
    {
        int sum = value.Select((c, i) => (c - '0') * multipliers[i]).Sum();
        int remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
