using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

/// <summary>
/// Manages the loading and storage of theme data from asset bundles to handle different game themes.
/// </summary>
public class ThemeDatabase
{
    static protected Dictionary<string, ThemeData> themeDataList; // Dictionary to store ThemeData objects with their theme names as keys.
    static public Dictionary<string, ThemeData> dictionnary { get { return themeDataList; } } // Public accessor for the dictionary.

    static protected bool m_Loaded = false; // Tracks whether the database has been loaded.
    static public bool loaded { get { return m_Loaded; } } // Public accessor for the loaded status.

    /// <summary>
    /// Retrieves the ThemeData for a given theme name.
    /// Returns null if the theme is not found or the database is not loaded.
    /// </summary>
    /// <param name="type">The name of the theme.</param>
    /// <returns>The ThemeData for the specified theme, or null if not found.</returns>
    static public ThemeData GetThemeData(string type)
    {
        ThemeData list;
        if (themeDataList == null || !themeDataList.TryGetValue(type, out list))
            return null;

        return list;
    }

    /// <summary>
    /// Loads all ThemeData objects from the asset bundles and populates the themeDataList dictionary.
    /// This method uses Unity's Addressables system for efficient asset management.
    /// </summary>
    /// <returns>An IEnumerator to allow this method to be used as a coroutine.</returns>
    static public IEnumerator LoadDatabase()
    {
        // If the dictionary is not null, it means the database was already loaded.
        if (themeDataList == null)
        {
            themeDataList = new Dictionary<string, ThemeData>();

            // Load all ThemeData assets tagged with "themeData" and populate the dictionary.
            yield return Addressables.LoadAssetsAsync<ThemeData>("themeData", op =>
            {
                if (op != null)
                {
                    if (!themeDataList.ContainsKey(op.themeName))
                        themeDataList.Add(op.themeName, op); // Add theme to dictionary if it's not already present.
                }
            });

            m_Loaded = true; // Mark the database as loaded.
        }
    }
}
