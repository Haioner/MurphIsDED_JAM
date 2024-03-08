using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FlipController : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float flipTargetDistance;
    [SerializeField] private List<Transform> ignoreChildren;
    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();
    private Vector2 m_input;
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        foreach (Transform child in ignoreChildren)
        {
            originalScales[child] = child.localScale;
        }
    }

    private void Update()
    {
        if (playerManager.playerState == PlayerState.Attack || playerManager.playerState == PlayerState.Die) return;

        if (GetNearestTarget() == null)
            FlipMovement();
        else
            FlipToTarget();
    }

    private Transform GetNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, flipTargetDistance, targetLayer);
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

        if (playerManager.playerState == PlayerState.Dash)
            return null;
        return nearestTarget;
    }

    public void MoveInput(InputAction.CallbackContext value)
    {
        m_input = value.ReadValue<Vector2>();
    }

    private void FlipMovement()
    {
        if (m_input.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            LockChildrenScaleX(true);
        }
        else if (m_input.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            LockChildrenScaleX(false);
        }
    }

    private void FlipToTarget()
    {
        if (GetNearestTarget().transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            LockChildrenScaleX(true);
        }
        else if (GetNearestTarget().transform.position.x > transform.position.x)
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
}
