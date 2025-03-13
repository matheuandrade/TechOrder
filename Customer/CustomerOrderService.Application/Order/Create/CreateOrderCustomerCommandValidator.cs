using CustomerOrderService.Application.Dto;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CustomerOrderService.Application.Order.Create;

internal sealed class CreateOrderCustomerCommandValidator : AbstractValidator<OrderDto>
{
    public CreateOrderCustomerCommandValidator()
    {
        RuleFor(c => c.CPF)
            .NotEmpty().NotNull().WithMessage("CPF é obrigatorio.")
            .Must(BeValidCpf).WithMessage("CPF Invalido.");

        RuleForEach(c => c.Products)
                .ChildRules(product =>
                {
                    product.RuleFor(a => a.ProductReference)
                        .NotEmpty().NotNull().WithMessage("Referencia do produto é obrigatória.")
                        .MaximumLength(30).WithMessage("Referencia do produto invalida.");

                    product.RuleFor(a => a.Quantity)
                        .NotEmpty().NotNull().WithMessage("Quantidade é obrigatória.")
                        .GreaterThan(0).WithMessage("Informe uma quantidade valida.");
                });

        RuleFor(c => c.Products)
            .Must(MinimumQuantity).WithMessage("Deve haver um pedido minimo de 1000 unidades.");

    }

    private static bool MinimumQuantity(List<OrderItemDto> products)
    {
        return products?.Sum(x => x.Quantity) >= 1000;
    }

    private static bool BeValidCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = Regex.Replace(cpf, "[^0-9]", ""); // Remove non-numeric characters

        if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false; // Avoid sequences like 111.111.111-11

        return ValidateCpfChecksum(cpf);
    }

    private static bool ValidateCpfChecksum(string cpf)
    {
        int[] firstMultiplier = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] secondMultiplier = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        int firstSum = cpf[..9].Select((digit, index) => (digit - '0') * firstMultiplier[index]).Sum();
        int firstDigit = (firstSum * 10) % 11;
        if (firstDigit == 10) firstDigit = 0;
        if (firstDigit != (cpf[9] - '0')) return false;

        int secondSum = cpf[..10].Select((digit, index) => (digit - '0') * secondMultiplier[index]).Sum();
        int secondDigit = (secondSum * 10) % 11;
        if (secondDigit == 10) secondDigit = 0;
        if (secondDigit != (cpf[10] - '0')) return false;

        return true;
    }
}
