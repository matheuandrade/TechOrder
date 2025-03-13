using FluentValidation;
using ResellerService.Aplication.Dtos;
using System.Text.RegularExpressions;

namespace ResellerService.Aplication.Reseller.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<ResellerDto>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.CNPJ)
            .NotEmpty().NotNull().WithMessage("CNP é obrigatorio.")
            .Length(14).WithMessage("CNPJ deve conter 14 digitos.")
            .Matches(@"^\d+$").WithMessage("CNPJ deve conter somente números.")
            .Must(IsValidCNPJ).WithMessage("CNPJ inválido.");

        RuleFor(c => c.CorporateName)
            .NotEmpty().NotNull().WithMessage("A Razão Social é obrigatória.")
            .MinimumLength(3).WithMessage("A Razão Social deve ter pelo menos 3 caracteres.")
            .MaximumLength(150).WithMessage("A Razão Social deve ter no máximo 150 caracteres.")
            .Matches(@"^[\p{L}0-9\s\.,\-&]+$").WithMessage("A Razão Social contém caracteres inválidos.");

        RuleFor(c => c.TradeName)
            .NotEmpty().NotNull().WithMessage("O Nome Fantasia é obrigatório.")
            .MinimumLength(3).WithMessage("O Nome Fantasia deve ter pelo menos 3 caracteres.")
            .MaximumLength(100).WithMessage("O Nome Fantasia deve ter no máximo 100 caracteres.");

        RuleFor(c => c.Email)
            .NotEmpty().NotNull().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O E-mail deve ser válido.")
            .Must(HasValidDomain).WithMessage("O e-mail deve ter um domínio válido.");

        RuleFor(c => c.Phones)
            .Must(phones => phones.All(phone => IsValidPhoneNumber(phone.PhoneAddres)))
            .WithMessage("Os números de telefone devem ser válidos.");

        RuleFor(c => c.Contacts)
            .NotEmpty().NotNull().WithMessage("Pelo menos um contato deve ser cadastrado.")
            .Must(HaveAtLeastOnePrincipal).WithMessage("Deve haver um contato principal.");

        RuleFor(c => c.Addresses)
            .NotEmpty().NotNull().WithMessage("Pelo menos um endereço de entrega deve ser cadastrado.");

        RuleForEach(c => c.Addresses)
                .ChildRules(address =>
                {
                    address.RuleFor(a => a.StreetAddress)
                        .NotEmpty().WithMessage("A rua do endereço é obrigatória.")
                        .MaximumLength(200).WithMessage("O nome da rua não pode ter mais de 200 caracteres.");

                    address.RuleFor(a => a.Number)
                        .NotEmpty().WithMessage("O número do endereço é obrigatório.");

                    address.RuleFor(a => a.City)
                        .NotEmpty().WithMessage("A cidade do endereço é obrigatória.")
                        .MaximumLength(100).WithMessage("O nome da cidade não pode ter mais de 100 caracteres.");

                    address.RuleFor(a => a.ZipCode)
                        .NotEmpty().WithMessage("O CEP do endereço é obrigatório.")
                        .Matches(@"^\d{5}-\d{3}$").WithMessage("O formato do CEP é inválido (ex: 12345-678).");
                });
    }

    private static bool HaveAtLeastOnePrincipal(List<ContactsDto> contactList) 
        => contactList.Any(c => c.IsPrimary);

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        var phonePattern = @"^(\+55\s?)?(\(?\d{2}\)?\s?)?\d{4,5}[-\s]?\d{4}$";
        return Regex.IsMatch(phoneNumber, phonePattern);
    }

    private bool HasValidDomain(string email)
    {
        // Verificar se o e-mail contém um domínio válido (Exemplo: .com, .org)
        var domain = email.Split('@').LastOrDefault();
        return !string.IsNullOrEmpty(domain) && domain.Contains('.');
    }

    private bool IsValidCNPJ(string cnpj)
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
