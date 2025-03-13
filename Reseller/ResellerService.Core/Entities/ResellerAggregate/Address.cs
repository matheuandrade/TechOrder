using ResellerService.Core.Primitives;

namespace ResellerService.Core.Entities.ResellerAggregate;

public class Address : Entity
{
    private Address(string streetAddress,
                    string number,
                    string zipCode,
                    string complement,
                    string city,
                    string state)
    {
        StreetAddress = streetAddress;
        Number = number;
        ZipCode = zipCode;
        Complement = complement;     
        City = city;
        State = state;
    }

    public static Address Create(string streetAddress,
                    string number,
                    string zipCode,
                    string complement,
                    string city,
                    string state)
    {
        return new Address(streetAddress, number, zipCode, complement, city, state);
    }

    public string StreetAddress { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string Complement { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;
}
