using UnityEngine;

public class InputDataTracker : MonoBehaviour
{
    private int leftCount = 0;
    private int rightCount = 0;
    private int upCount = 0;
    private int downCount = 0;

    void Update()
    {
        // Check for player input and track the number of presses
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftCount++;
            Debug.Log("Left pressed: " + leftCount);
            DataStorageHandler.SaveInputData("Left");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightCount++;
            Debug.Log("Right pressed: " + rightCount);
            DataStorageHandler.SaveInputData("Right");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upCount++;
            Debug.Log("Up pressed: " + upCount);
            DataStorageHandler.SaveInputData("Up");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            downCount++;
            Debug.Log("Down pressed: " + downCount);
            DataStorageHandler.SaveInputData("Down");
        }
    }
}
