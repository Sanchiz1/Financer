﻿namespace Domain.Common;

public abstract class Entity<T>
{
    public T Id { get; init; }
}