using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthRegenText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI dashCooldownText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI cooldownAccelerationText;

    [Header("Items")]


    [Header("Attacks")]
    [SerializeField] private List<AttackInventorySlot> attackList = new List<AttackInventorySlot>();

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
}
