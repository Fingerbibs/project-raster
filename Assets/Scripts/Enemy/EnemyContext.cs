using UnityEngine;
public class EnemyContext
{
    public readonly EnemyController Enemy;
    public readonly Transform Transform;
    
    public readonly EnemyPatrol EnemyPatrol;

    public EnemyContext(EnemyController enemy, Transform transform, EnemyPatrol enemyPatrol)
    {
        Enemy     = enemy;
        Transform  = transform;

        EnemyPatrol = enemyPatrol;
    }
}