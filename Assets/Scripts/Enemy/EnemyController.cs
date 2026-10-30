using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private StateMachine<EnemyState, EnemyContext> stateMachine;
    private EnemyPatrol enemyPatrol;
    private EnemyVision enemyVision;
    private EnemyChase enemyChase;
    private EnemySearch enemySearch;
    private NavMeshAgent agent;

    private EnemyState previousState = EnemyState.Patrol;

    public event Action<EnemyController, Vector3> OnAlerted;

    public Vector3 SearchTarget { get; private set; }

    private void Awake()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyVision = GetComponent<EnemyVision>();
        enemyChase  = GetComponent<EnemyChase>();
        enemySearch = GetComponent<EnemySearch>();
        agent       = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        enemyVision.OnPlayerSpotted  += HandlePlayerSpotted;
        enemyChase.OnTargetLost      += HandleTargetLost;
        enemySearch.OnSearchFinished += HandleSearchFinished;
    }

    private void OnDisable()
    {
        enemyVision.OnPlayerSpotted  -= HandlePlayerSpotted;
        enemyChase.OnTargetLost      -= HandleTargetLost;
        enemySearch.OnSearchFinished -= HandleSearchFinished;
    }

    // Init in Start so every system's Awake has run before the first state's Enter()
    private void Start() => InitStateMachine();

    private void Update() => stateMachine.Update();

    private void HandlePlayerSpotted()
    {
        EnemyState state = GetState();
        if (state != EnemyState.Patrol && state != EnemyState.Evasion) return;

        SetState(EnemyState.Alert);
        OnAlerted?.Invoke(this, enemyVision.LastKnownPlayerPosition);
    }

    private void HandleTargetLost()
    {
        if (GetState() != EnemyState.Alert) return;

        SearchTarget = enemyVision.LastKnownPlayerPosition;
        SetState(EnemyState.Evasion);
    }

    // Called by AlertZone when another enemy in the zone spots the player
    public void Investigate(Vector3 position)
    {
        EnemyState state = GetState();
        if (state == EnemyState.Alert) return;

        SearchTarget = position;
        if (state == EnemyState.Evasion) enemySearch.StartSearch(position);
        else if (state == EnemyState.Patrol) SetState(EnemyState.Evasion);
    }

    private void HandleSearchFinished()
    {
        if (GetState() == EnemyState.Evasion) SetState(EnemyState.Patrol);
    }

    #region State Machine
    private void InitStateMachine()
    {
        var context = new EnemyContext(
            this,
            transform,
            enemyPatrol,
            enemyVision,
            enemyChase,
            enemySearch
        );


        var states = new Dictionary<EnemyState, IState<EnemyState>>
        {
            { EnemyState.Patrol,        new PatrolState(context) },
            { EnemyState.Alert,         new AlertState(context) },
            { EnemyState.Evasion,       new EvasionState(context) },
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

    public bool IsWalking() => agent.velocity.sqrMagnitude > 0.01f;
    public bool IsIdle() => !IsWalking();
    #endregion
}
