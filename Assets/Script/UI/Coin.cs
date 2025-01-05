using UnityEngine;

/// <summary>
/// Represents a coin in the game. Handles its type (regular or premium) and utilizes object pooling for optimization.
/// </summary>
public class Coin : MonoBehaviour
{
    static public Pooler coinPool; // Static reference to a Pooler, which manages the recycling of Coin objects for optimized memory usage.
    public bool isPremium = false; // Indicates whether this coin is a premium coin or a regular one.
}
