using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPivot;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private UnityEvent attackEvent;
    private float currentAttackCooldown;
    private int currentAttackIndex;

    private PlayerManager playerManager;
    private GameInputs gameInputs;
    private GameData gameData;
    private float currentRange;
    private int savedCurrentAttackIndex;

    private void Awake()
    {
        gameInputs = new GameInputs();
        gameInputs.Enable();
    }

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        gameData = DataManager.instance.gameData;
        currentRange = gameData.Attacks[currentAttackIndex].AttackRange;
    }

    private void Update()
    {
        CalculateAttackCooldown();
        if (gameInputs.Player.Attack.ReadValue<float>() > 0)
            Attack();
    }

    private void CalculateAttackCooldown()
    {
        if (currentAttackCooldown > 0)
            currentAttackCooldown -= Time.deltaTime;
    }

    public void Attack()
    {
        if(currentAttackCooldown <= 0)
        {
            playerManager.playerState = PlayerState.Attack;
            NextAttack();
            attackEvent?.Invoke();
        }
    }

    private void NextAttack()
    {
        // Animation
        string attackName = gameData.Attacks[currentAttackIndex].AttackName;
        playerManager.anim.Play(attackName);

        // Next equipped attack
        currentAttackIndex++;
        currentAttackIndex %= gameData.Attacks.Count;
        while (!gameData.Attacks[currentAttackIndex].AttackEquipped)
        {
            currentAttackIndex++;
            currentAttackIndex %= gameData.Attacks.Count;
        }
        gameData.NextAttack(currentAttackIndex);

        // Cooldown
        currentAttackCooldown = gameData.Attacks[currentAttackIndex].AttackCooldown;
    }

    public void AttackEvent()
    {
        DamageNearestEnemies();
        currentRange = gameData.Attacks[currentAttackIndex].AttackRange;
        savedCurrentAttackIndex = currentAttackIndex;
    }

    public void BackToIdleState()
    {
        playerManager.playerState = PlayerState.Idle;
    }

    private void DamageNearestEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentRange, targetLayer);
        int maxEnemiesToDamage = gameData.Attacks[savedCurrentAttackIndex].TargetsCount;
        int currentEnemiesDamaged = 0;

        System.Array.Sort(colliders, (c1, c2) =>
        {
            float distance1 = Vector2.SqrMagnitude(c1.transform.position - transform.position);
            float distance2 = Vector2.SqrMagnitude(c2.transform.position - transform.position);
            return distance1.CompareTo(distance2);
        });

        foreach (Collider2D collider in colliders)
        {
            if (currentEnemiesDamaged >= maxEnemiesToDamage)
                break;

            collider.GetComponentInChildren<HealthController>().Damage(gameData.Attacks[currentAttackIndex].AttackDamage);
            currentEnemiesDamaged++;
        }
    }
}
