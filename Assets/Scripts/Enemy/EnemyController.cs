using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private StateMachine<EnemyState, EnemyContext> stateMachine;
    private EnemyPatrol enemyPatrol;
    private EnemyVision enemyVision;

    private EnemyState previousState = EnemyState.Patrol;

    private void Awake()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyVision = GetComponent<EnemyVision>();
    }

    private void OnEnable() => enemyVision.OnPlayerSpotted += HandlePlayerSpotted;
    private void OnDisable() => enemyVision.OnPlayerSpotted -= HandlePlayerSpotted;

    // Init in Start so every system's Awake has run before the first state's Enter()
    private void Start() => InitStateMachine();

    private void Update() => stateMachine.Update();

    private void HandlePlayerSpotted()
    {
        if (GetState() == EnemyState.Patrol) SetState(EnemyState.Alert);
    }

    #region State Machine
    private void InitStateMachine()
    {
        var context = new EnemyContext(
            this,
            transform,
            enemyPatrol,
            enemyVision
        );


        var states = new Dictionary<EnemyState, IState<EnemyState>>
        {
            { EnemyState.Patrol,        new PatrolState(context) },
            { EnemyState.Alert,         new AlertState(context) },
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

    public bool IsWalking() => GetState() == EnemyState.Patrol && !IsIdle();
    public bool IsIdle() => GetState() == EnemyState.Alert || enemyPatrol.IsWaiting();
    #endregion
}
