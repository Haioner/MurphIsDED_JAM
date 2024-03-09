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

    private void Start()
    {
        UpdateSlotInfo();
    }

    public void SetActiveSelectSlot(bool state)
    {
        selectHolder.SetActive(state);
    }

    public void SelectSlot()
    {
        PlayerAttacks eqquipedAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
        if (eqquipedAttack.AttackLevel >= eqquipedAttack.MaxAttackLevel && upgradeManager.SelectedAttack.AttackName == eqquipedAttack.AttackName)
                return;

        //DataManager.instance.gameData.UnlockNextGameLevel();
        //DataManager.instance.gameData.NextGameLevel();
        //upgradeManager.gameController.ResetGameController();
        UpgradeAttack();
        UpdateSlotInfo();
        upgradeManager.FinishUpgrade();
        //upgradeManager.attacksHolder.gameObject.SetActive(false);
    }

    private void UpgradeAttack()
    {
        //Select Slot
        PlayerAttacks eqquipedAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
        if (eqquipedAttack.AttackName == null || upgradeManager.SelectedAttack.AttackName != eqquipedAttack.AttackName)
        {
            DataManager.instance.gameData.ConvertAndEquipAttack(upgradeManager.SelectedAttack, slotIndex);
        }
        else
        {
            DataManager.instance.gameData.UpgradeAttack(slotIndex);
        }

        //DataManager.instance.SaveData();
    }

    public void UpdateSlotInfo()
    {
        if (DataManager.instance.gameData.EqquipedAttacks[slotIndex] != null)
            if (DataManager.instance.gameData.EqquipedAttacks[slotIndex].AttackName == null
                || string.IsNullOrEmpty(DataManager.instance.gameData.EqquipedAttacks[slotIndex].AttackName))
                EmptySlot();
            else
            {
                PlayerAttacks eqquipedAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
                nameText.SetText(eqquipedAttack.AttackName);
                iconImage.sprite = DataManager.instance.gameData.Attacks[eqquipedAttack.AttackIndex].AttackIcon;
                if (eqquipedAttack.AttackLevel >= eqquipedAttack.MaxAttackLevel)
                    levelText.SetText("MAX");
                else
                    levelText.SetText("Lvl " + eqquipedAttack.AttackLevel.ToString());
            }
    }

    private void EmptySlot()
    {
        nameText.SetText("Empty Slot");
        levelText.SetText("");
        iconImage.sprite = emptyImage;
    }
}
