using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 m_input;

    private void FixedUpdate()
    {
        Vector2 movement = m_input * speed * Time.deltaTime;
        Vector2 newPosition = rb.position + movement;
        rb.MovePosition(newPosition);
    }

    public void MoveInput(InputAction.CallbackContext value)
    {
        m_input = value.ReadValue<Vector2>();
    }
}
