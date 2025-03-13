namespace ResellerService.Core.Primitives;

public abstract class Entity
{
    public Guid Id { get; private init; } = Guid.NewGuid();
}
