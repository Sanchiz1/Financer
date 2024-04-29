namespace Domain.Abstractions;

public abstract class BaseEntity<T>
{
    public T Id { get; init; }
}