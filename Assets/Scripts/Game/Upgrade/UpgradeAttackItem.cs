using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAttackItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI attackLevelText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;

    private UpgradeManager upgradeManager;
    private AttackSO playerAttack;
    private int itemIndex;

    public void InitiateItem(UpgradeManager upgradeManager, int attackIndex, int itemIndex)
    {
        this.upgradeManager = upgradeManager;
        playerAttack = DataManager.instance.gameData.Attacks[attackIndex];
        this.itemIndex = itemIndex;
        SetItemInfo();
    }

    private void SetItemInfo()
    {
        attackLevelText.SetText(playerAttack.AttackLevel.ToString());
        nameText.SetText(playerAttack.AttackName);
        descriptionText.SetText(playerAttack.AttackDescription);
        iconImage.sprite = playerAttack.AttackIcon;
    }

    public void SelectUpgradeItem()
    {
        upgradeManager.SelectedAttack = playerAttack;
        upgradeManager.inventoryManager.SetActiveAttackSlotsSelection(true);
        upgradeManager.UpdateSelectedItemColor(itemIndex);
    }

    public void SetItemNormalColor(Color color)
    {
        ColorBlock colorBlock = GetComponent<Button>().colors;
        colorBlock.normalColor = color;
        GetComponent<Button>().colors = colorBlock;
    }
}
