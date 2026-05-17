using UnityEngine;

public class PatrolState : IState<EnemyState>
{
    private readonly EnemyContext c;

    public PatrolState(EnemyContext context) => c = context;

    public void Enter() { }

    public void Update() { }

    public void Exit() { }

    public bool CanTransitionTo(EnemyState next) => true;
}