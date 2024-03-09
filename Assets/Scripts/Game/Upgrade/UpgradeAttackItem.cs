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

    [Header("Sprites")]
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private Sprite cardSelectedSprite;
    [SerializeField] private Image cardBackground;

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
        upgradeManager.StartAttackSelection();
        upgradeManager.UpdateSelectedItemColor(itemIndex);
    }

    public void SetItemSelectionSprite(bool isSelected)
    {
        if (isSelected)
            cardBackground.sprite = cardSprite;
        else
            cardBackground.sprite = cardSelectedSprite;
        
    }
}
