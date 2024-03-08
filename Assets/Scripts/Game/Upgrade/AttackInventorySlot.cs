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

    //private AttackSO playerAttack;

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
        //playerAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
    }

    public void SelectSlot()
    {
        PlayerAttacks eqquipedAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
        if (eqquipedAttack.AttackLevel >= eqquipedAttack.MaxAttackLevel)
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
        PlayerAttacks eqquipedAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
        if (eqquipedAttack.AttackName == null || upgradeManager.SelectedAttack.AttackName != eqquipedAttack.AttackName)
        {
            //playerAttack = upgradeManager.SelectedAttack;
            //DataManager.instance.gameData.EqquipedAttacks[slotIndex] = playerAttack;
            DataManager.instance.gameData.ConvertAndEquipAttack(upgradeManager.SelectedAttack, slotIndex);
           // playerAttack = DataManager.instance.gameData.Attacks[slotIndex];

            //DataManager.instance.gameData.UpgradeAttack(slotIndex);
        }
        //else if (upgradeManager.SelectedAttack.AttackName != eqquipedAttack.AttackName)
        //{
        //    //playerAttack = upgradeManager.SelectedAttack;
        //    //DataManager.instance.gameData.EqquipedAttacks[slotIndex] = playerAttack;
        //    DataManager.instance.gameData.ConvertAndEquipAttack(upgradeManager.SelectedAttack,slotIndex);
        //    //playerAttack = DataManager.instance.gameData.Attacks[slotIndex];
        //}
        else
        {
            DataManager.instance.gameData.UpgradeAttack(slotIndex);
        }

        DataManager.instance.SaveData();
    }

    public void UpdateSlotInfo()
    {
        if (DataManager.instance.gameData.EqquipedAttacks[slotIndex].AttackName == null 
            || string.IsNullOrEmpty(DataManager.instance.gameData.EqquipedAttacks[slotIndex].AttackName) )
            EmptySlot();
        else
        {
            PlayerAttacks eqquipedAttack = DataManager.instance.gameData.EqquipedAttacks[slotIndex];
            nameText.SetText(eqquipedAttack.AttackName);
            //iconImage.sprite = eqquipedAttack.AttackIcon;
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
