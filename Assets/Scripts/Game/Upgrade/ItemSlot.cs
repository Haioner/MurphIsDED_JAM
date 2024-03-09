using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private ItemSO item;
    [SerializeField] private Sprite emptySprite;
    private int itemIndex = -1;

    private void OnEnable()
    {
        InitiateItem();
    }

    private void InitiateItem()
    {
        EmptyItem();
        GetItemIndex();
        SetItemInfo();
    }

    private void EmptyItem()
    {
        nameText.SetText("Empty" + item.ItemList[0].itemType);
        statsText.SetText("");
        itemIcon.sprite = emptySprite;
    }

    private void SetItemInfo()
    {
        if (itemIndex < 0) return;

        nameText.SetText(item.ItemList[itemIndex].ItemName);
        itemIcon.sprite = item.ItemList[itemIndex].Icon;
        ItemStats();
    }

    private void ItemStats()
    {
        string damage = "";
        string health = "";
        string healthRegen = "";
        string speed = "";
        string cooldownAccelerator = "";
        string dashCD = "";

        if (item.ItemList[itemIndex].DamageItem != 0)
            damage = ColorizeText("Damage: ", item.ItemList[itemIndex].DamageItem);

        if (item.ItemList[itemIndex].HealthItem != 0)
            health = ColorizeText("Health: ", item.ItemList[itemIndex].HealthItem);

        if (item.ItemList[itemIndex].HealthRegenItem != 0)
            healthRegen = ColorizeText("Health Regen: ", item.ItemList[itemIndex].HealthRegenItem);

        if (item.ItemList[itemIndex].SpeedItem != 0)
            speed = ColorizeText("Speed: ", item.ItemList[itemIndex].SpeedItem);

        if (item.ItemList[itemIndex].AttackCooldownAccelerator != 0)
            cooldownAccelerator = ColorizeText("CD Accelerator: ", item.ItemList[itemIndex].AttackCooldownAccelerator);

        if (item.ItemList[itemIndex].DashCooldownItem != 0)
            dashCD = ColorizeText("Dash Cooldown: ", item.ItemList[itemIndex].DashCooldownItem);

        statsText.text = damage + health + healthRegen + speed + cooldownAccelerator + dashCD;
    }

    private string ColorizeText(string prefix, float value)
    {
        // Define a cor com base no sinal do valor
        string colorTag = value >= 0 ? "<color=#00FF00>" : "<color=#FF0000>";
        // Retorna o texto formatado com a cor definida
        return colorTag + prefix + value.ToString() + "</color>\n";
    }

    private void GetItemIndex()
    {
        ItemType itemType = item.ItemList[0].itemType;
        switch (itemType)
        {
            case ItemType.MeeleWeapon: itemIndex = DataManager.instance.itemData.CurrentWeaponIndex; break;
            case ItemType.Helmet: itemIndex = DataManager.instance.itemData.CurrentHelmetIndex; break;
            case ItemType.Chestplate: itemIndex = DataManager.instance.itemData.CurrentChestplateIndex; break;
            case ItemType.Boots: itemIndex = DataManager.instance.itemData.CurrentBootsIndex; break;
        }
    }
}
