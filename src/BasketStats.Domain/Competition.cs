using System;

namespace BasketStats.Domain;

public class Competition
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    // Private constructor for EF Core
    private Competition() { }

    public static Competition Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Competition name cannot be empty.", nameof(name));
        }

        return new Competition
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}
