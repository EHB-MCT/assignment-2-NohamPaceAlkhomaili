using UnityEngine;

/// <summary>
/// Sets a random integer parameter on the animator when the state is entered.
/// </summary>
public class RandomAnimation : StateMachineBehaviour
{
    public string parameter; // Name of the animator parameter to set
    public int count; // Upper limit for the random value (exclusive)

    /// <summary>
    /// Called when the animator enters a new state.
    /// </summary>
    /// <param name="animator">The animator controlling the state machine</param>
    /// <param name="stateInfo">Information about the current animator state</param>
    /// <param name="layerIndex">Index of the current animator layer</param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set the parameter to a random value between 0 (inclusive) and count (exclusive)
        animator.SetInteger(parameter, Random.Range(0, count));
    }
}
