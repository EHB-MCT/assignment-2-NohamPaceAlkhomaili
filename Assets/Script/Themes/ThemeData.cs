using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Represents a segment within a theme, specifying the length and possible prefabs for that segment.
/// </summary>
[System.Serializable]
public struct ThemeZone
{
    public int length;                // Length of the zone (number of track segments).
    public AssetReference[] prefabList; // List of prefab references for the zone.
}

/// <summary>
/// This scriptable object defines all the data related to a theme in the game.
/// It is stored as an asset in the project and can be bundled into an asset bundle.
/// </summary>
[CreateAssetMenu(fileName = "themeData", menuName = "Trash Dash/Theme Data")]
public class ThemeData : ScriptableObject
{
    [Header("Theme Data")]
    public string themeName;          // Name of the theme.
    public int cost;                  // Cost to unlock the theme using regular currency.
    public int premiumCost;           // Cost to unlock the theme using premium currency.
    public Sprite themeIcon;          // Icon representing the theme in the UI.

    [Header("Objects")]
    public ThemeZone[] zones;         // Array of zones defining the structure of the theme.
    public GameObject collectiblePrefab; // Prefab for the collectible item in the theme.
    public GameObject premiumCollectible; // Prefab for the premium collectible item in the theme.

    [Header("Decoration")]
    public GameObject[] cloudPrefabs; // Array of cloud prefabs used for visual decoration.
    public Vector3 cloudMinimumDistance = new Vector3(0, 20.0f, 15.0f); // Minimum distance between clouds.
    public Vector3 cloudSpread = new Vector3(5.0f, 0.0f, 1.0f);         // Spread factor for cloud positioning.
    public int cloudNumber = 10;      // Number of clouds to spawn in the theme.
    public Mesh skyMesh;              // Mesh used for rendering the sky in this theme.
    public Mesh UIGroundMesh;         // Mesh used for rendering the ground in the UI preview.
    public Color fogColor;            // Fog color for atmospheric effects in this theme.
}
