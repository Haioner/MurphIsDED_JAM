using UnityEngine;
using System.Collections.Generic;

public class FlipController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private List<Transform> ignoreChildren;
    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();

    private void Start()
    {
        mainCamera = Camera.main;

        foreach (Transform child in ignoreChildren)
        {
            originalScales[child] = child.localScale;
        }
    }

    private void Update()
    {
        Flip();
    }

    private void Flip()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float playerDirection = mousePosition.x - transform.position.x;

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
}
