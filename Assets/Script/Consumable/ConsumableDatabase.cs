using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The consumable database stores a list of consumable items as a project asset.
/// Designers can manually assign prefabs for each consumable to control which items are available in the game.
/// This approach allows explicit control over the database's contents compared to automatic population.
/// </summary>
[CreateAssetMenu(fileName="Consumables", menuName = "Trash Dash/Consumables Database")]
public class ConsumableDatabase : ScriptableObject
{
    // Array of consumables defined in the project
    public Consumable[] consumbales;

    // Static dictionary mapping each consumable type to its corresponding consumable object
    static protected Dictionary<Consumable.ConsumableType, Consumable> _consumablesDict;

    /// <summary>
    /// Initializes the consumables dictionary if it hasn't already been loaded.
    /// Maps each consumable type to its respective consumable object.
    /// </summary>
    public void Load()
    {
        if (_consumablesDict == null)
        {
            _consumablesDict = new Dictionary<Consumable.ConsumableType, Consumable>();

            for (int i = 0; i < consumbales.Length; ++i)
            {
                _consumablesDict.Add(consumbales[i].GetConsumableType(), consumbales[i]);
            }
        }
    }

    /// <summary>
    /// Retrieves a consumable object based on its type.
    /// </summary>
    /// <param name="type">The type of the consumable to retrieve</param>
    /// <returns>The consumable object if found, otherwise null</returns>
    static public Consumable GetConsumbale(Consumable.ConsumableType type)
    {
        Consumable c;
        return _consumablesDict.TryGetValue(type, out c) ? c : null;
    }
}
