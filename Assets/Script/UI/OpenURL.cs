using UnityEngine;

/// <summary>
/// This script opens a specified URL when triggered (e.g., via a button click).
/// </summary>
public class OpenURL : MonoBehaviour
{
    public string websiteAddress; // The URL to be opened.

    /// <summary>
    /// Opens the specified website address in the default web browser.
    /// This method can be linked to a UI button.
    /// </summary>
    public void OpenURLOnClick()
    {
        Application.OpenURL(websiteAddress); // Opens the given URL in the default web browser.
    }
}
