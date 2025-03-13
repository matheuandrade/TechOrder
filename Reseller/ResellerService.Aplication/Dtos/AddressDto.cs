namespace ResellerService.Aplication.Dtos;

public class AddressDto
{
    public string StreetAddress { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string Complement { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;
}
