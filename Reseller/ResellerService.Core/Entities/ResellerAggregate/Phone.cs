using ResellerService.Core.Primitives;

namespace ResellerService.Core.Entities;

public class Phone : Entity
{
    private Phone(string phoneAddres)
    {
        PhoneAddres = phoneAddres;
    }

    public static Phone Create(string phoneAddres)
    {
        return new Phone(phoneAddres);
    }

    public string PhoneAddres { get; set; } = null!;
}
