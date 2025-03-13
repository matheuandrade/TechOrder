namespace ResellerService.Aplication.Dtos;

public class ResellerDto
{
    public string CNPJ { get; set; } = null!;

    //Razão social
    public string CorporateName { get; set; } = null!;

    //Nome Fantasia
    public string TradeName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public List<PhoneDto> Phones { get; set; } = [];

    public List<ContactsDto> Contacts { get; set; } = [];

    public List<AddressDto> Addresses { get; set; } = [];
}
