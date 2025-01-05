using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for implementing obstacles in the game.
/// Derived classes must define the specific behavior for spawning obstacles.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class Obstacle : MonoBehaviour
{
    /// <summary>
    /// Sound effect to play when the obstacle is impacted.
    /// </summary>
    public AudioClip impactedSound;

    /// <summary>
    /// Optional setup method for initializing the obstacle. Can be overridden by derived classes.
    /// </summary>
    public virtual void Setup() {}

    /// <summary>
    /// Abstract method to handle obstacle spawning logic.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="segment">The track segment where the obstacle will be placed.</param>
    /// <param name="t">The normalized position (0 to 1) along the segment.</param>
    /// <returns>An IEnumerator for asynchronous operations.</returns>
    public abstract IEnumerator Spawn(TrackSegment segment, float t);

    /// <summary>
    /// Handles the visual and audio feedback when the obstacle is impacted by the player.
    /// Plays an animation and sound if available.
    /// </summary>
    public virtual void Impacted()
    {
        // Get the Animation component from any child object
        Animation anim = GetComponentInChildren<Animation>();
        
        // Get the AudioSource component attached to this obstacle
        AudioSource audioSource = GetComponent<AudioSource>();

        // Play the animation if it exists
        if (anim != null)
        {
            anim.Play();
        }

        // Play the impacted sound if AudioSource and sound are available
        if (audioSource != null && impactedSound != null)
        {
            audioSource.Stop();
            audioSource.loop = false;  // Ensure the sound does not loop
            audioSource.clip = impactedSound;
            audioSource.Play();
        }
    }
}
