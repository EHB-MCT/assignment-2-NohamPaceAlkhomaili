using UnityEngine;

public class RestartRunning : StateMachineBehaviour
{
    static int s_DeadHash = Animator.StringToHash("Dead"); // Hash for the "Dead" animator parameter

    /// <summary>
    /// Called when exiting a state in the animator.
    /// </summary>
    /// <param name="animator">The animator controlling the state machine</param>
    /// <param name="stateInfo">Information about the current animator state</param>
    /// <param name="layerIndex">Index of the current animator layer</param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If the "Dead" parameter is true, do nothing and return
        if (animator.GetBool(s_DeadHash))
            return; 
        
        // Restart movement in the TrackManager if not going to the death state
        TrackManager.instance.StartMove();
    }
}
