using System.Collections.Generic;
using UnityEngine;

public class MovementStateMachine
{
    private IMovementState current;
    private Dictionary<MovementState, IMovementState> states;

    public MovementState CurrentState { get; private set; }

    public MovementStateMachine(Dictionary<MovementState, IMovementState> states, MovementState initialState)
    {
        this.states = states;
        CurrentState = initialState;
        current = states[initialState];
        current.Enter();
    }

    public void Update()
    {
        if (current != null)
            current.Update();
    }

    public bool TransitionTo(MovementState next)
    {
        if (CurrentState == next) return false;

        if (!current.CanTransitionTo(next))
        {
            Debug.LogWarning($"Transition blocked: {CurrentState} → {next}");
            return false;
        }

        Debug.Log($"State changed: {CurrentState} → {next}");
        current.Exit();
        CurrentState = next;
        current = states[next];
        current.Enter();

        return true;
    }
}