using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MobileCanvasManager : MonoBehaviour
{
    [Header("DASH")]
    [SerializeField] private Image dashCooldownImage;
    private float currentDashCooldown;

    [Header("ATTACK")]
    [SerializeField] private Image attackCooldownImage;
    [SerializeField] private UnityEvent holdAttackEvent;
    private float currentAttackCooldown;
    private bool attackHold;

    GameData gameData;

    private void Start()
    {
        gameData = DataManager.instance.gameData;
    }

    private void Update()
    {
        CalculateDashCooldownImage();
        CalculateAttackCooldownImage();
        UpdateHoldAttack();
    }

    //Dash
    private void CalculateDashCooldownImage()
    {
        dashCooldownImage.fillAmount = currentDashCooldown / gameData.DashCooldown;

        if (currentDashCooldown > 0)
            currentDashCooldown -= Time.deltaTime;
    }

    public void StartDashCooldown()
    {
        if (currentDashCooldown <= 0)
            currentDashCooldown = gameData.DashCooldown;
    }

    //Attack
    private void CalculateAttackCooldownImage()
    {
        attackCooldownImage.fillAmount = currentAttackCooldown / gameData.Attacks[gameData.Attack].AttackCooldown;

        if (currentAttackCooldown > 0)
            currentAttackCooldown -= Time.deltaTime;
    }

    private void UpdateHoldAttack()
    {
        if (attackHold)
        {
            holdAttackEvent?.Invoke();
        }
    }

    public void StartAttackCooldown()
    {
        if (currentAttackCooldown <= 0)
            currentAttackCooldown = gameData.Attacks[gameData.Attack].AttackCooldown;
    }

    public void SetAttackHoldActive(bool state)
    {
        attackHold = state;
    }
}
