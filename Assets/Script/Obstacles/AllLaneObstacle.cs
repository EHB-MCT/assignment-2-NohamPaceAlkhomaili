using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Represents an obstacle that spans all lanes on the track.
/// Uses Unity's Addressables system to asynchronously instantiate the obstacle.
/// </summary>
public class AllLaneObstacle : Obstacle
{
    /// <summary>
    /// Spawns the obstacle at a specific point on the track segment based on a normalized time parameter.
    /// </summary>
    /// <param name="segment">The track segment where the obstacle will be placed.</param>
    /// <param name="t">The normalized position (0 to 1) along the segment.</param>
    /// <returns>An IEnumerator to handle the asynchronous instantiation process.</returns>
    public override IEnumerator Spawn(TrackSegment segment, float t)
    {
        Vector3 position;
        Quaternion rotation;

        // Retrieve the position and rotation at the specified point on the segment
        segment.GetPointAt(t, out position, out rotation);

        // Instantiate the obstacle asynchronously using Addressables
        AsyncOperationHandle op = Addressables.InstantiateAsync(gameObject.name, position, rotation);
        yield return op;

        // Check if the instantiation was successful
        if (op.Result == null || !(op.Result is GameObject))
        {
            Debug.LogWarning(string.Format("Unable to load obstacle {0}.", gameObject.name));
            yield break;
        }

        // Set the instantiated obstacle as a child of the segment's object root
        GameObject obj = op.Result as GameObject;
        obj.transform.SetParent(segment.objectRoot, true);

        // TODO: Temporary workaround for issue #7
        // Adjust the position slightly and revert to fix a potential misalignment problem
        Vector3 oldPos = obj.transform.position;
        obj.transform.position += Vector3.back; // Move slightly backward
        obj.transform.position = oldPos;        // Revert to the original position
    }
}
