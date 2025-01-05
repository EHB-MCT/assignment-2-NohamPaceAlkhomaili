using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script provides a simple method to load a scene by its name.
/// </summary>
public class LevelLoader : MonoBehaviour
{
    /// <summary>
    /// Loads a scene based on the given scene name.
    /// </summary>
    /// <param name="name">The name of the scene to load.</param>
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name); // Uses the SceneManager to load the specified scene.
    }
}
