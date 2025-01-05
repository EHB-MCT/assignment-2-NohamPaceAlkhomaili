using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Helper script to assign an object's audio source output to the main mixer group.
/// This avoids duplicating the mixer in the bundle, ensuring the object uses the main mixer.
/// </summary>
public class AssignOutputChannel : MonoBehaviour
{
    public string mixerGroup;  // Name of the mixer group to assign the audio source to.

    /// <summary>
    /// Assigns the audio source's output to the specified mixer group on Awake.
    /// If the audio source or mixer group is missing, appropriate error messages are logged.
    /// </summary>
    private void Awake()
    {
        // Get the AudioSource component attached to this GameObject.
        AudioSource source = GetComponent<AudioSource>();

        // Log an error and destroy this script if no AudioSource is found.
        if (source == null)
        {
            Debug.LogError("That object doesn't have any audio source, can't change its output", gameObject);
            Destroy(this);
            return;
        }

        // Find all mixer groups matching the specified name.
        AudioMixerGroup[] groups = MusicPlayer.instance.mixer.FindMatchingGroups(mixerGroup);

        // Log an error if no matching mixer group is found.
        if (groups.Length == 0)
        {
            Debug.LogErrorFormat(gameObject, "Could not find any group called {0}", mixerGroup);
        }

        // Assign the matching mixer group to the AudioSource's output.
        for (int i = 0; i < groups.Length; ++i)
        {
            if (groups[i].name == mixerGroup)
            {
                source.outputAudioMixerGroup = groups[i];
                break;
            }
        }
    }
}
