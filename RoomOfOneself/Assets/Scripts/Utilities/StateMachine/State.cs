using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the base class of a state, should be attach to an object that is a child object of a state machine
/// </summary>
public class State : MonoBehaviour
{
    /// <summary>
    /// the id of this state, used for identify
    /// </summary>
    [SerializeField]
    protected string stateID;
    public string StateID
    {
        get
        {
            return stateID;
        }
    }

    /// <summary>
    /// the statemachine this state is belong to
    /// </summary>
    [SerializeField]
    protected StateMachine stateMachine;

    /// <summary>
    /// the transitions this state contains
    /// </summary>
    protected List<StateTransition> transitions;
    public List<StateTransition> Transitions
    {
        get
        {
            return transitions;
        }
    }

    /// <summary>
    /// if the conditions should be check everyframe when the state is active
    /// </summary>
    [SerializeField]
    protected bool alwaysCheckConditions = false;
    public bool AlwaysCheckConditions
    {
        get
        {
            return alwaysCheckConditions;
        }
    }


    /// <summary>
    /// public method to add a transition
    /// </summary>
    /// <param name="stateTransition"></param>
    public void AddTransitionToState(StateTransition stateTransition)
    {
        if (transitions == null)
            transitions = new List<StateTransition>();

        if (transitions.Contains(stateTransition))
            return;

        transitions.Add(stateTransition);
    }

    /// <summary>
    /// public method to remove a transition
    /// </summary>
    /// <param name="stateTransition"></param>
    public void RemoveTransition(StateTransition stateTransition)
    {
        if (transitions == null)
            return;

        if (!transitions.Contains(stateTransition))
            return;

        transitions.Remove(stateTransition);
    }


    public virtual void DoInState()
    {
        // to be override 
        // what to do when it's in this state
    }

    public virtual void DoWhenEnter()
    {
        // to be override 
        // what to do when entering this state
    }

    public virtual void DoWhenExit()
    {
        // to be override 
        // what to do when exit this state
    }

}

