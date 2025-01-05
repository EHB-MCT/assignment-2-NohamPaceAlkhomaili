using UnityEngine;

/// <summary>
/// Stores data related to a character's accessory. 
/// Attached to child objects of the Character Prefab in Bundles/Characters.
/// </summary>
public class CharacterAccessories : MonoBehaviour
{
    public string accessoryName; // Name of the accessory
    public int cost; // Cost to unlock the accessory
    public int premiumCost; // Premium currency cost to unlock
    public Sprite accessoryIcon; // Icon representing the accessory
}
