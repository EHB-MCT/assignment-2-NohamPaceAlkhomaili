using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Handles the spawning logic for simple barricade obstacles on the track.
/// </summary>
public class SimpleBarricade : Obstacle
{
    protected const int k_MinObstacleCount = 1;  // Minimum number of obstacles per spawn
    protected const int k_MaxObstacleCount = 2;  // Maximum number of obstacles per spawn
    protected const int k_LeftMostLaneIndex = -1;  // Leftmost lane index
    protected const int k_RightMostLaneIndex = 1;  // Rightmost lane index
    
    /// <summary>
    /// Spawns the barricade(s) on the track at a specific position.
    /// Handles both normal and tutorial-specific spawning logic.
    /// </summary>
    /// <param name="segment">The track segment where the barricade will be spawned.</param>
    /// <param name="t">The normalized position (0 to 1) along the segment.</param>
    /// <returns>An IEnumerator for asynchronous operations.</returns>
    public override IEnumerator Spawn(TrackSegment segment, float t)
    {
        // Special case for the tutorial: spawn a single, centered barricade to guide the player.
        bool isTutorialFirst = TrackManager.instance.isTutorial && TrackManager.instance.firstObstacle && segment == segment.manager.currentSegment;

        if (isTutorialFirst)
            TrackManager.instance.firstObstacle = false;

        // Determine the number of obstacles to spawn and the starting lane.
        int count = isTutorialFirst ? 1 : Random.Range(k_MinObstacleCount, k_MaxObstacleCount + 1);
        int startLane = isTutorialFirst ? 0 : Random.Range(k_LeftMostLaneIndex, k_RightMostLaneIndex + 1);

        // Get the spawn position and rotation on the track.
        Vector3 position;
        Quaternion rotation;
        segment.GetPointAt(t, out position, out rotation);

        // Spawn each obstacle and place them in the correct lane.
        for (int i = 0; i < count; ++i)
        {
            int lane = startLane + i;
            lane = lane > k_RightMostLaneIndex ? k_LeftMostLaneIndex : lane;  // Wrap around lanes if out of bounds.

            // Asynchronously instantiate the obstacle.
            AsyncOperationHandle op = Addressables.InstantiateAsync(gameObject.name, position, rotation);
            yield return op;

            // Handle loading failure.
            if (op.Result == null || !(op.Result is GameObject))
            {
                Debug.LogWarning(string.Format("Unable to load obstacle {0}.", gameObject.name));
                yield break;
            }

            GameObject obj = op.Result as GameObject;

            if (obj == null)
            {
                Debug.Log(gameObject.name);
            }
            else
            {
                // Adjust the position of the obstacle based on the lane.
                obj.transform.position += obj.transform.right * lane * segment.manager.laneOffset;

                // Set the obstacle as a child of the track's object root.
                obj.transform.SetParent(segment.objectRoot, true);

                // Temporary workaround for a known issue (#issue7).
                Vector3 oldPos = obj.transform.position;
                obj.transform.position += Vector3.back;
                obj.transform.position = oldPos;
            }
        }
    }
}
