using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyManager enemyManager;
    private Transform playerTransform;
    private float currentSpeed;
    private Vector2 dashDirection = Vector2.zero;
    private PlayerManager playerManager;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        playerTransform = enemyManager.player.transform;
        currentSpeed = GetRandomSpeed();
        playerManager = enemyManager.player.GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        if (playerManager.playerState == PlayerState.Die)
        {
            enemyManager.enemyState = EnemyState.Idle;
        }

        HitKnockback();
        UpdateDash();

        if (enemyManager.enemyState == EnemyState.Chase)
            MoveToTarget();
        else if (enemyManager.enemyState == EnemyState.GetAway)
            MoveBackwards();
    }

    private float GetRandomSpeed()
    {
        return Random.Range(enemyManager.enemySO.MinMaxSpeed.x, enemyManager.enemySO.MinMaxSpeed.y);
    }

    private float GetTargetDistance()
    {
        if (enemyManager.Target != null)
            return Vector2.Distance(transform.position, enemyManager.Target.position);
        else
            return 999;
    }

    private void MoveToTarget()
    {
        if (enemyManager.Target == null) return;
        if (GetTargetDistance() > enemyManager.enemySO.AttacksList[enemyManager.CurrentAttack].AttackRange)
        {
            //Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
            Vector2 direction = ((Vector2)enemyManager.Target.position - (Vector2)transform.position).normalized;
            transform.Translate(direction * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void MoveBackwards()
    {
        Vector2 direction = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
        transform.Translate(direction * (currentSpeed * 1.75f) * Time.fixedDeltaTime);
    }

    private void HitKnockback()
    {
        if (enemyManager.enemyState == EnemyState.Hit)
        {
            Vector2 direction = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
            transform.Translate(direction * (currentSpeed * 3.5f) * Time.fixedDeltaTime);
        }
    }

    public void UpdateDash()
    {
        if (enemyManager.enemyState == EnemyState.Dash)
        {
            if (dashDirection != Vector2.zero)
            {
                transform.Translate(dashDirection * (currentSpeed * 3.5f) * Time.fixedDeltaTime);
            }
            else
            {
                dashDirection = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
                dashDirection *= 3f;
            }
        }
        else
            dashDirection = Vector2.zero;
    }


}
