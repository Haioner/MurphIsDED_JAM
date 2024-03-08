using System.Collections;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    //Attack
    [SerializeField] private Transform attackPivot;
    private int currentAttack = -1;
    private float currentAttackCooldown;
    private LayerMask targetLayer;

    //GetAway
    private bool canGetAway;
    private float currentGetAwayTimer;

    //Cache
    private EnemyManager enemyManager;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        currentGetAwayTimer = GetAwayTimer();
        NextAttack();
    }

    private void Update()
    {
        if (enemyManager.enemyState == EnemyState.Die) return;
        if (GetTargetHealthController().GetCurrentHealth() <= 0) return;

        UpdateAttack();
        CalculateCooldown();
        CalculateGetAway();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyManager.enemyState == EnemyState.Die ) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            GetTargetHealthController().Damage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyManager.enemyState == EnemyState.Die || enemyManager.enemyState == EnemyState.Spawn) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            GetTargetHealthController().Damage(enemyManager.enemySO.AttacksList[currentAttack].Damage);
        }
    }

    private float GetAwayTimer()
    {
        return Random.Range(enemyManager.enemySO.MinMaxGetAwayTimer.x, enemyManager.enemySO.MinMaxGetAwayTimer.y);
    }

    private Transform GetNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Mathf.Infinity, targetLayer);
        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = collider.transform;
            }
        }

        if (nearestTarget == null)
            return enemyManager.player.transform;
        else
            return nearestTarget;
    }

    private HealthController GetTargetHealthController()
    {
        return GetNearestTarget().GetComponentInChildren<HealthController>();
    }

    private float GetTargetDistance()
    {
        if (GetNearestTarget() != null)
            return Vector2.Distance(transform.position, GetNearestTarget().position);
        else
            return Vector2.Distance(transform.position, enemyManager.player.transform.position);
    }

    private bool IsTargetNear()
    {
        return GetTargetDistance() <= enemyManager.enemySO.AttacksList[enemyManager.CurrentAttack].AttackRange;
    }

    private AttackType GetAttackType()
    {
        return enemyManager.enemySO.attackType;
    }

    private void UpdateAttack()
    {
        if(IsTargetNear() && enemyManager.enemyState == EnemyState.Chase && !canGetAway)
        {
            enemyManager.enemyState = EnemyState.Idle;
        }
        if(!IsTargetNear() && enemyManager.enemyState == EnemyState.Idle)
        {
            enemyManager.enemyState = EnemyState.Chase;
        }
    }

    private string GetAttackAnimName()
    {
        return "Attack" + currentAttack;
    }

    private void CalculateCooldown()
    {
        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
        else if(enemyManager.enemyState == EnemyState.Idle)
        {
            enemyManager.anim.Play(GetAttackAnimName());
            enemyManager.enemyState = EnemyState.Attack;
        }
    }

    public void AttackEvent()
    {
        if (GetAttackType() == AttackType.Ranged)
        {
            AttackRanged();
        }

        if (GetAttackType() == AttackType.Meele)
        {
            AttackMeele();
        }
        
        SetGetAway();
        NextAttack();
    }

    private void AttackMeele()
    {
        if (enemyManager.enemyState == EnemyState.Die) return;

        if (GetTargetDistance() <= enemyManager.enemySO.AttacksList[enemyManager.CurrentAttack].AttackRange)
            GetTargetHealthController().Damage(enemyManager.enemySO.AttacksList[currentAttack].Damage);

        if (enemyManager.enemyState != EnemyState.Dash)
            enemyManager.enemyState = EnemyState.Chase;
    }

    private void AttackRanged()
    {
        if (enemyManager.enemyState == EnemyState.Die) return;

        BulletController bullet = Instantiate(enemyManager.enemySO.AttacksList[currentAttack].bullet, attackPivot.position, Quaternion.identity);
        bullet.InitiateBullet(GetNearestTarget(), targetLayer, enemyManager.enemySO.AttacksList[currentAttack].Damage);
        enemyManager.enemyState = EnemyState.Chase;
    }

    private void NextAttack()
    {
        if (currentAttack < enemyManager.enemySO.AttacksList.Count - 1)
            currentAttack++;
        else
            currentAttack = 0;

        enemyManager.CurrentAttack = currentAttack;
        currentAttackCooldown = enemyManager.enemySO.AttacksList[currentAttack].AttackCooldown;
        targetLayer = enemyManager.enemySO.AttacksList[currentAttack].TargetLayer;
    }

    public void AttackDashEvent(float dashTime)
    {
        StartCoroutine(DashTimer(dashTime));
    }

    private IEnumerator DashTimer(float time)
    {
        enemyManager.enemyState = EnemyState.Dash;
        yield return new WaitForSeconds(time);
        if (enemyManager.enemyState != EnemyState.Die)
        {
            enemyManager.enemyState = EnemyState.Idle;
            NextAttack();
        }
    }

    private void SetGetAway()
    {
        if (enemyManager.enemySO.isBrave) return;
        canGetAway = GetRandomGetAway();
    }

    private bool GetRandomGetAway()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }

    private void CalculateGetAway()
    {
        if (canGetAway)
        {
            enemyManager.enemyState = EnemyState.GetAway;
            currentGetAwayTimer -= Time.deltaTime;

            if(currentGetAwayTimer < 0)
            {
                enemyManager.enemyState = EnemyState.Chase;
                canGetAway = false;
                currentGetAwayTimer = GetAwayTimer();
            }
        }
    }
}
