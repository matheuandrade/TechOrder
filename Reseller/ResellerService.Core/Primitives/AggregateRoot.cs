namespace ResellerService.Core.Primitives;

public interface IAggregateRoot { }

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    protected AggregateRoot() 
    {
    }
}
