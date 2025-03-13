using ResellerService.Core.Primitives;

namespace ResellerService.Core.Entities.ResellerAggregate;

public class Contacts : Entity
{
    private Contacts(string name, bool isPrimary)
    {
        Name = name;
        IsPrimary = isPrimary;
    }

    public static Contacts Create(string name, bool isPrimary)
    {
        return new Contacts(name, isPrimary);
    }

    public string Name { get; set; } = null!;

    public bool IsPrimary { get; set; } = false;
}
