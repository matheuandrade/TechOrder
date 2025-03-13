using ResellerService.Core.Primitives;

namespace ResellerService.Core.Entities.ResellerAggregate;

public class Reseller : AggregateRoot
{
    private Reseller(string cnpj,
                     string corporateName,
                     string tradeName,
                     string email,
                     List<Phone> phones,
                     List<Contacts> contacts,
                     List<Address> addresses)
    {
        CNPJ = cnpj;
        CorporateName = corporateName;
        TradeName = tradeName;
        Email = email;
        Phones = phones;
        Contacts = contacts;
        Addresses = addresses;           
    }

    public Reseller() { }

    public static Reseller Create(
                     string cnpj,
                     string corporateName,
                     string tradeName,
                     string email,
                     List<Phone> phones,
                     List<Contacts> contacts,
                     List<Address> addresses)
    {
        return new Reseller(cnpj,
                            corporateName,
                            tradeName,
                            email,
                            phones,
                            contacts,
                            addresses);
    }

    public string CNPJ { get; set; } = null!;

    public string CorporateName { get; set; } = null!;

    public string TradeName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public List<Phone> Phones { get; set; } = [];

    public List<Contacts> Contacts { get; set; } = [];

    public List<Address> Addresses { get; set; } = [];
}
