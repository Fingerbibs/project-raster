using UnityEngine;

public class PatrolState : IState<EnemyState>
{
    private readonly EnemyContext c;

    public PatrolState(EnemyContext context) => c = context;

    public void Enter() => c.EnemyPatrol.StartPatrol();

    public void Update() => c.EnemyPatrol.Patrol();

    public void Exit() { }

    public bool CanTransitionTo(EnemyState next) => true;
}