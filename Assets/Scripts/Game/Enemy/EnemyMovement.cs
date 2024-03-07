using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyManager enemyManager;
    private Transform playerTransform;
    private float currentSpeed;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        playerTransform = enemyManager.player.transform;
        currentSpeed = GetRandomSpeed();
    }

    private void Update()
    {
        HitStopMovement();
    }

    private void FixedUpdate()
    {
        if (enemyManager.enemyState == EnemyState.Chase)
            MoveToPlayer();
        else if (enemyManager.enemyState == EnemyState.GetAway)
            MoveBackwards();
    }

    private float GetRandomSpeed()
    {
        return Random.Range(enemyManager.enemySO.MinMaxSpeed.x, enemyManager.enemySO.MinMaxSpeed.y);
    }

    private float GetPlayerDistance()
    {
        return Vector2.Distance(transform.position, playerTransform.position);
    }

    private void MoveToPlayer()
    {
        if (GetPlayerDistance() >  enemyManager.enemySO.StopRange)
        {
            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
            transform.Translate(direction * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void MoveBackwards()
    {
        Vector2 direction = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
        transform.Translate(direction * (currentSpeed * 1.75f) * Time.fixedDeltaTime);
    }

    private void HitStopMovement()
    {
        if (enemyManager.enemyState == EnemyState.Hit)
        {
            Vector2 direction = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
            transform.Translate(direction * (currentSpeed * 3.5f) * Time.fixedDeltaTime);
        }
    }
}
