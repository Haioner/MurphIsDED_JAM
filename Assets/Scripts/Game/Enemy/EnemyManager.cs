using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum EnemyState
{
    Spawn, Idle, Attack, Die, Chase, GetAway, Hit
}

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy CACHE")]
    public EnemyState enemyState;
    [SerializeField] private HealthController healthController;
    [SerializeField] private string enemyLayer;
    [SerializeField] private List<Transform> ignoreChildren;
    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();

    [HideInInspector]public Animator anim;
    [HideInInspector] public EnemySO enemySO;
    [HideInInspector] public GameObject player;

    private void Start()
    {
        foreach (Transform child in ignoreChildren)
        {
            originalScales[child] = child.localScale;
        }

        healthController.enabled=false;
        healthController.SetMaxHealth(enemySO.Health);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enemyState != EnemyState.Die && enemyState != EnemyState.Spawn)
            Flip();
    }

    private void Flip()
    {
        float playerDirection = player.transform.position.x - transform.position.x;

        if (playerDirection < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            LockChildrenScaleX(true);
        }
        else if (playerDirection > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            LockChildrenScaleX(false);
        }
    }

    private void LockChildrenScaleX(bool locked)
    {
        foreach (Transform child in ignoreChildren)
        {
            if (locked)
            {
                Vector3 originalScale = originalScales[child];
                float sign = Mathf.Sign(transform.localScale.x);
                child.localScale = new Vector3(sign * originalScale.x, child.localScale.y, child.localScale.z);
            }
            else
            {
                child.localScale = originalScales[child];
            }
        }
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

        int layerNumber = LayerMask.NameToLayer(enemyLayer);
        gameObject.layer = layerNumber;
    }

    public void DieEnemy()
    {
        enemyState = EnemyState.Die;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void HitEnemyState()
    {
        enemyState = EnemyState.Hit;
        StartCoroutine(BackToIdleState());
    }

    private IEnumerator BackToIdleState()
    {
        yield return new WaitForSeconds(0.125f);
        if (enemyState != EnemyState.Die)
            enemyState = EnemyState.Idle;
    }

}
