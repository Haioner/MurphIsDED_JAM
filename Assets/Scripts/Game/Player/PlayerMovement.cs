using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private float dashStopTime = 0.15f;
    [SerializeField] private UnityEvent dashCooldownEvent;

    [Header("CACHE")]
    [SerializeField] private Rigidbody2D rb;

    private Vector2 m_input;
    private GameData gameData;
    private GameInputs controls;
    private bool isDashing = false;
    private PlayerManager playerManager;

    private void Awake()
    {
        controls = new GameInputs();
        controls.Player.Dash.performed += e => Dash();
        controls.Enable();
    }

    private void Start()
    {
        gameData = DataManager.instance.gameData;
        playerManager = GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        if (playerManager.playerState == PlayerState.Die) return;
        Move();
    }

    public void MoveInput(InputAction.CallbackContext value)
    {
        m_input = value.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector2 movement = m_input * GetSpeed() * Time.deltaTime;
        transform.position += new Vector3(movement.x, movement.y, 0f);
        MovementState();
    }

    private void MovementState()
    {
        if (playerManager.playerState != PlayerState.Dash && playerManager.playerState != PlayerState.Attack)
        {
            if (m_input.magnitude > 0)
                playerManager.playerState = PlayerState.Walk;
            else
                playerManager.playerState = PlayerState.Idle;
        }
    }

    private float GetSpeed()
    {
        if (playerManager.playerState != PlayerState.Attack)
            return gameData.Speed;
        else
            return gameData.EqquipedAttacks[gameData.Attack].AttackMovementSpeed;
    }

    public void Dash()
    {
        if (!isDashing && playerManager.playerState != PlayerState.Die)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerDash");
            rb.velocity = Vector2.zero;

            Vector2 dashDirection = m_input.normalized;
            Vector2 dashForceVector;

            if (m_input.magnitude > 0.1f)
            {
                dashForceVector = dashDirection * gameData.DashForce;
            }
            else
            {
                dashForceVector = transform.right * gameData.DashForce;
            }

            rb.AddForce(dashForceVector, ForceMode2D.Impulse);
            isDashing = true;
            playerManager.playerState = PlayerState.Dash;

            StartCoroutine(DashStop());
            StartCoroutine(DashCooldown());
            dashCooldownEvent?.Invoke();
        }
    }

    private IEnumerator DashStop()
    {
        yield return new WaitForSeconds(dashStopTime);
        gameObject.layer = LayerMask.NameToLayer("Player");
        rb.velocity = Vector2.zero;
        playerManager.playerState = PlayerState.Idle;
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(gameData.DashCooldown);
        isDashing = false;
    }
}
