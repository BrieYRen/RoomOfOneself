using UnityEngine;

/// <summary>
/// this is a base transition class of a state machine
/// should be attach to an object that is the child object of another game object that is attached a state script
/// </summary>
public class StateTransition : MonoBehaviour
{
    /// <summary>
    /// the name of this transition
    /// </summary>
    [SerializeField]
    [Tooltip("The id of this state transition, used for identify")]
    protected string transitionID;
    public string TransitionID
    {
        get
        {
            return transitionID;
        }
    }

    /// <summary>
    /// set the state this transition belongs to in inspector
    /// </summary>
    [SerializeField]
    [Tooltip("The state which this transition is belong to")]
    protected State startState;

    /// <summary>
    /// set the target state this transition will lead to in inspector
    /// </summary>
    [SerializeField]
    [Tooltip("The state which this transition will lead to")]
    protected State targetState;

    public virtual bool TransitionCondition()
    {
        // to be override the conditions to transition into certain state

        return false;
    }

}
