using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Stores a database of all characters from bundles, indexed by their name.
/// </summary>
public class CharacterDatabase
{
    static protected Dictionary<string, Character> m_CharactersDict; // Dictionary to store characters by name

    static public Dictionary<string, Character> dictionary { get { return m_CharactersDict; } } // Accessor for the character dictionary

    static protected bool m_Loaded = false; // Tracks if the database is loaded
    static public bool loaded { get { return m_Loaded; } } // Accessor for load status

    /// <summary>
    /// Retrieves a character by name from the dictionary.
    /// </summary>
    /// <param name="type">Name of the character</param>
    /// <returns>The corresponding Character object, or null if not found</returns>
    static public Character GetCharacter(string type)
    {
        Character c;
        if (m_CharactersDict == null || !m_CharactersDict.TryGetValue(type, out c))
            return null;

        return c;
    }

    /// <summary>
    /// Loads the character database asynchronously from Addressables.
    /// </summary>
    /// <returns>Coroutine for loading the database</returns>
    static public IEnumerator LoadDatabase()
    {
        if (m_CharactersDict == null)
        {
            m_CharactersDict = new Dictionary<string, Character>(); // Initialize the dictionary

            // Load all character GameObjects labeled as "characters" in Addressables
            yield return Addressables.LoadAssetsAsync<GameObject>("characters", op =>
            {
                Character c = op.GetComponent<Character>(); // Get the Character component
                if (c != null)
                {
                    m_CharactersDict.Add(c.characterName, c); // Add to the dictionary
                }
            });

            m_Loaded = true; // Mark the database as loaded
        }
    }
}
