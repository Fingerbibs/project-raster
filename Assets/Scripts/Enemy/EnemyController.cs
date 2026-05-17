using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    private StateMachine<EnemyState, EnemyContext> stateMachine;

    private EnemyState previousState = EnemyState.Patrol;

    private void Awake()
    {
        InitStateMachine();
    }

    private void Update() => stateMachine.Update();

    #region State Machine
    private void InitStateMachine()
    {
        var context = new EnemyContext(
            this,
            transform
        );


        var states = new Dictionary<EnemyState, IState<EnemyState>>
        {
            { EnemyState.Patrol,        new PatrolState(context) },
            //{ EnemyState.Alert,       new AlertState(context) },
            //{ EnemyState.KO, new KOState(context) },
        };

        stateMachine = new StateMachine<EnemyState, EnemyContext>(states, EnemyState.Patrol);
    }

    public void SetState(EnemyState next)
    {
        previousState = stateMachine.CurrentState;
        stateMachine.TransitionTo(next);
    }

    public EnemyState GetState() => stateMachine.CurrentState;
    #endregion
}