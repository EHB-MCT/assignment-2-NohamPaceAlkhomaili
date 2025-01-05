using UnityEngine;
using System;

/// <summary>
/// Defines a character's data and properties. 
/// Attached to character prefabs in the Bundles/Characters folder.
/// </summary>
public class Character : MonoBehaviour
{
    public string characterName; // Name of the character
    public int cost; // Cost to unlock the character
    public int premiumCost; // Premium currency cost to unlock

    public CharacterAccessories[] accessories; // Array of available accessories for the character

    public Animator animator; // Animator component for character animations
    public Sprite icon; // Icon representing the character

    [Header("Sound")] // Grouping sound-related fields in the Unity Inspector
    public AudioClip jumpSound; // Sound played when the character jumps
    public AudioClip hitSound; // Sound played when the character gets hit
    public AudioClip deathSound; // Sound played when the character dies

    /// <summary>
    /// Activates the selected accessory based on the given index. 
    /// Disables all accessories if the index is -1.
    /// </summary>
    /// <param name="accessory">Index of the accessory to activate</param>
    public void SetupAccesory(int accessory)
    {
        for (int i = 0; i < accessories.Length; ++i)
        {
            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
        }
    }
}
