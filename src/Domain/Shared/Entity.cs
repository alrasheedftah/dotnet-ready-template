namespace Domain.Shared;

public interface IStrongId<TValue>
{
    TValue Value { get; }
}

public readonly record struct CustomerId(Guid Value) : IStrongId<Guid>
{
    public static CustomerId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}


public abstract class Entity<TId> : IEquatable<Entity<TId>>
{
    public TId Id  {get; protected set; } = default!;
    public bool IsTransient() => EqualityComparer<TId>.Default.Equals(Id, default!);
    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    public bool Equals(Entity<TId>? other)
    {
        if (other is  null) return false;

        // better for Avoids unnecessary checks
        // fast if they are same object  of course they are equals
        if (ReferenceEquals(this, other))
            return true;
        
        if (GetType() != other.GetType())
            return false;

        // this means if one of them is not persistence yet in db
        if (IsTransient() || other.IsTransient())
            return false;
        
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
        => IsTransient()
            ? base.GetHashCode()
            : HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !(left == right);            
}
