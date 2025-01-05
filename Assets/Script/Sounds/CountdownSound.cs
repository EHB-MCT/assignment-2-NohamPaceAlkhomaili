using UnityEngine;

/// <summary>
/// Plays a countdown sound with a slight delay, then disables the GameObject once the sound finishes playing.
/// </summary>
public class CountdownSound : MonoBehaviour
{
    protected AudioSource m_Source;  // Reference to the AudioSource component that plays the sound.
    protected float m_TimeToDisable; // Time remaining before disabling the GameObject.

    protected const float k_StartDelay = 0.5f; // Delay before the sound starts playing.

    /// <summary>
    /// Called when the GameObject is enabled.
    /// Initializes the AudioSource and starts playing the sound with a delay.
    /// </summary>
    void OnEnable()
    {
        // Get the AudioSource component attached to this GameObject.
        m_Source = GetComponent<AudioSource>();

        // Set the time to disable based on the length of the audio clip.
        m_TimeToDisable = m_Source.clip.length;

        // Play the sound after a small delay.
        m_Source.PlayDelayed(k_StartDelay);
    }

    /// <summary>
    /// Called every frame. Updates the countdown and disables the GameObject when the sound ends.
    /// </summary>
    void Update()
    {
        // Reduce the remaining time by the time elapsed since the last frame.
        m_TimeToDisable -= Time.deltaTime;

        // Disable the GameObject once the sound has finished playing.
        if (m_TimeToDisable < 0)
            gameObject.SetActive(false);
    }
}
