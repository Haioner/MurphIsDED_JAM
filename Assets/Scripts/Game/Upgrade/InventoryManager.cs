using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject inventoryHolder;

    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthRegenText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI dashCooldownText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI cooldownAccelerationText;

    [Header("Attacks")]
    [SerializeField] private List<AttackInventorySlot> attackList = new List<AttackInventorySlot>();

    private void Start()
    {
        UpdatePlayerStatsText();
    }

    public void SwitchInventoryHolderState()
    {
        inventoryHolder.SetActive(!inventoryHolder.activeInHierarchy);
    }

    public void SetActiveAttackSlotsSelection(bool state)
    {
        foreach (var item in attackList)
        {
            item.SetActiveSelectSlot(state);
        }
        UpdateAttackSlots();
    }

    private void UpdateAttackSlots()
    {
        foreach (var item in attackList)
        {
            item.UpdateSlotInfo();
        }
    }

    public void UpdatePlayerStatsText()
    {
        GameData gameData = DataManager.instance.gameData;
        healthText.SetText("HP " + gameData.Health.ToString());
        healthRegenText.SetText("HP Regen " + gameData.HealthRegen.ToString());
        speedText.SetText("Speed " + gameData.Speed.ToString());
        damageText.SetText("DMG Multiplier " + gameData.DamageMultiplier.ToString());
        dashCooldownText.SetText("Dash CDR " + gameData.DashCooldown.ToString());
        cooldownAccelerationText.SetText("Attack CDR " + gameData.AttackCooldownSpeed.ToString());
    }
}
