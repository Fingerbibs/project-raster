using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TState, TContext> where TState : System.Enum
{
    private IState<TState> current;
    private Dictionary<TState, IState<TState>> states;

    public TState CurrentState { get; private set; }

    public StateMachine(Dictionary<TState, IState<TState>> states, TState initialState)
    {
        this.states = states;
        CurrentState = initialState;
        current = states[initialState];
        current.Enter();
    }

    public void Update() => current?.Update();

    public bool TransitionTo(TState next)
    {
        if (CurrentState.Equals(next)) return false;

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