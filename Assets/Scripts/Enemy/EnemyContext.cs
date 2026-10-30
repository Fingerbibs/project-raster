using UnityEngine;
public class EnemyContext
{
    public readonly EnemyController Enemy;
    public readonly Transform Transform;

    public readonly EnemyPatrol EnemyPatrol;
    public readonly EnemyVision EnemyVision;

    public EnemyContext(EnemyController enemy, Transform transform, EnemyPatrol enemyPatrol, EnemyVision enemyVision)
    {
        Enemy     = enemy;
        Transform  = transform;

        EnemyPatrol = enemyPatrol;
        EnemyVision = enemyVision;
    }
}