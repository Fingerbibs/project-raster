using UnityEngine;

public class AlertZone : MonoBehaviour
{
    private EnemyController[] enemies;

    private void Awake()
    {
        enemies = GetComponentsInChildren<EnemyController>();
    }

    private void OnEnable()
    {
        foreach (var enemy in enemies)
            enemy.OnAlerted += HandleEnemyAlerted;
    }

    private void OnDisable()
    {
        foreach (var enemy in enemies)
            enemy.OnAlerted -= HandleEnemyAlerted;
    }

    private void HandleEnemyAlerted(EnemyController source, Vector3 position)
    {
        foreach (var enemy in enemies)
        {
            if (enemy != source) enemy.Investigate(position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (var enemy in GetComponentsInChildren<EnemyController>())
            Gizmos.DrawLine(transform.position, enemy.transform.position);
    }
}
