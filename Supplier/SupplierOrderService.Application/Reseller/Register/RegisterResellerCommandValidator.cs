using FluentValidation;
using SupplierOrderService.Application.Dtos;

namespace SupplierOrderService.Application.Reseller.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<ResellerDto>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.CNPJ)
            .NotEmpty().NotNull().WithMessage("CNP é obrigatorio.")
            .Length(14).WithMessage("CNPJ deve conter 14 digitos.")
            .Matches(@"^\d+$").WithMessage("CNPJ deve conter somente números.")
            .Must(IsValidCNPJ).WithMessage("CNPJ inválido.");
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
