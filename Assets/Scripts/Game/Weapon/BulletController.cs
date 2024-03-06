using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool canFollowTarget;
    [SerializeField] private float maxErrorAngle = 5f;
    private LayerMask targetLayerMask;
    private Transform target;
    private float damage;

    public void InitiateBullet(Transform target, LayerMask layer, float damage)
    {
        this.target = target;
        targetLayerMask = layer;
        this.damage = damage;
        RotateToTarget();
        Destroy(gameObject, 8f);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (target == null) return;

        MoveToTarget();
        MoveToTargetDirection();
    }

    private void RotateToTarget()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 0.7f, transform.position.z);
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float randomError = Random.Range(-maxErrorAngle, maxErrorAngle);
        angle += randomError;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void MoveToTarget()
    {
        if (canFollowTarget)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 0.7f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, bulletSpeed * Time.deltaTime);
        }
    }

    private void MoveToTargetDirection()
    {
        if (!canFollowTarget)
            transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((targetLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            collision.GetComponentInChildren<HealthController>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
