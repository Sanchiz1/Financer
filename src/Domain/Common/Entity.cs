namespace Domain.Abstractions;

public abstract class Entity<T>
{
    public T Id { get; init; }
}