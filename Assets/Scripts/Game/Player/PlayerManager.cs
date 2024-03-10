using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerState
{
    Idle, Attack, Die, Walk, Run, Dash
}

public class PlayerManager : MonoBehaviour
{
    public PlayerState playerState;
    [SerializeField] private HealthController healthController;
    [HideInInspector] public Animator anim;
    private GameData gameData;

    [Header("Target arrow")]
    [SerializeField] private Transform targetArrow;
    [SerializeField] private LayerMask targetLayer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameData = DataManager.instance.gameData;
        UpdateMaxHealth();
    }

    private void Update()
    {
        RotateArrowToTarget();
    }

    public void UpdateMaxHealth()
    {
        healthController.UpdateRegenValue(gameData.HealthRegen);
        healthController.SetMaxHealth(gameData.Health);
    }

    public void SetDieState()
    {
        playerState = PlayerState.Die;
    }

    private void RotateArrowToTarget()
    {
        if (GetNearestTarget() != null)
        {
            Vector3 targetPosition = new Vector3(GetNearestTarget().position.x, GetNearestTarget().position.y + 0.7f, targetArrow.position.z);
            Vector2 direction = (targetPosition - targetArrow.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetArrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            targetArrow.gameObject.SetActive(true);
        }
        else
        {
            targetArrow.gameObject.SetActive(false);
        }
    }

    private Transform GetNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Mathf.Infinity, targetLayer);
        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.transform != transform)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = collider.transform;
                }
            }
        }

        if (nearestTarget == null)
            return null;
        else if (nearestDistance < 5)
            return null;
        else
            return nearestTarget;
    }
}
