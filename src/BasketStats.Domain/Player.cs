// src/BasketStats.Domain/Player.cs
namespace BasketStats.Domain;

public class Player
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private Player() { }

    public static Player Create(string name)
    {
        // Add validation as needed
        return new Player { Id = Guid.NewGuid(), Name = name };
    }
}