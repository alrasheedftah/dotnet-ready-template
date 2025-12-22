namespace Application.Shared.Pipeline;

public interface IOrderedBehavior
{
    int Order { get; }
}


public static class BehaviorOrder
{
    public const int Validation = 100;
    public const int Logging    = 200;
    public const int Transaction = 300;
    public const int Outbox     = 400;
    public const int SaveChanges = 500;
}
