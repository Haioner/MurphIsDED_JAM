using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackInventorySlot : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private UpgradeManager upgradeManager;

    [Header("Slot Info")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite emptyImage;

    [Header("Select")]
    [SerializeField] private GameObject selectHolder;

    private PlayerAttacks playerAttack;

    private void Start()
    {
        InitiatePlayerAttack();
        UpdateSlotInfo();
    }

    public void SetActiveSelectSlot(bool state)
    {
        selectHolder.SetActive(state);
    }

    private void InitiatePlayerAttack()
    {
        playerAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
    }

    public void SelectSlot()
    {
        if (playerAttack.AttackLevel >= playerAttack.MaxAttackLevel)
            return;

        DataManager.instance.gameData.UnlockNextGameLevel();
        DataManager.instance.gameData.NextGameLevel();
        upgradeManager.gameController.ResetGameController();
        UpgradeAttack();
        UpdateSlotInfo();
        upgradeManager.inventoryManager.gameObject.SetActive(false);
    }

    private void UpgradeAttack()
    {
        //Select Slot
        if (upgradeManager.SelectedAttack.AttackName != playerAttack.AttackName)
        {
            playerAttack = upgradeManager.SelectedAttack.Clone();
            DataManager.instance.gameData.EqquipedAttacks[slotIndex] = playerAttack;
        }
        else
        {
            DataManager.instance.gameData.UpgradeAttack(slotIndex);
        }

        DataManager.instance.SaveData();
    }

    public void UpdateSlotInfo()
    {
        EmptySlot();

        if (playerAttack.AttackName == null) return;

        nameText.SetText(playerAttack.AttackName);
        iconImage.sprite = playerAttack.AttackIcon;
        if (playerAttack.AttackLevel >= playerAttack.MaxAttackLevel)
            levelText.SetText("MAX");
        else
            levelText.SetText("Lvl " + playerAttack.AttackLevel.ToString());
    }

    private void EmptySlot()
    {
        if (playerAttack.AttackName == null)
        {
            nameText.SetText("Empty Slot");
            levelText.SetText("");
            iconImage.sprite = emptyImage;
        }
    }
}
