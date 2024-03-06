using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum EnemyState
{
    Idle, Attack, Die, Chase, GetAway
}

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy CACHE")]
    public EnemyState enemyState;
    [SerializeField] private HealthController healthController;

    [HideInInspector]public Animator anim;
    [HideInInspector] public EnemySO enemySO;
    [HideInInspector] public GameObject player;

    private void Start()
    {
        healthController.enabled=false;
        healthController.SetMaxHealth(enemySO.Health);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enemyState != EnemyState.Die && enemyState != EnemyState.Idle)
            Flip();
    }

    private void Flip()
    {
        float playerDirection = player.transform.position.x - transform.position.x;

        if (playerDirection < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (playerDirection > 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void InitiateEnemy(EnemySO enemy, GameObject player, UnityEvent dieEvent)
    {
        enemySO = enemy;
        this.player = player;
        healthController.DieEvent.AddListener(dieEvent.Invoke);
    }

    public void EnableEnemy()
    {
        enemyState = EnemyState.Chase;
        healthController.enabled=true;
    }

    public void DieEnemy()
    {
        enemyState = EnemyState.Die;
    }
}
