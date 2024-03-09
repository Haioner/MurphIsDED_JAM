using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ItemShop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private Image iconImage;

    private ItemSO item;
    private UpgradeManager upgradeManager;
    private int itemIndex;

    public void InitiateItem(ItemSO itemSO, UpgradeManager upgradeManager, int itemIndex)
    {
        item = itemSO;
        this.upgradeManager = upgradeManager;
        this.itemIndex = itemIndex;
        SetItemInfo();
    }

    private void SetItemInfo()
    {
        if (item == null) return;
        nameText.SetText(item.ItemList[itemIndex].ItemName);
        priceText.SetText(item.ItemList[itemIndex].ItemPrice.ToString());
        iconImage.sprite = item.ItemList[itemIndex].Icon;
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
        {
            float equippedValue = GetEquippedValue(item.ItemList[itemIndex].itemType, "DamageItem");
            damage = ColorizeText("Damage: ", item.ItemList[itemIndex].DamageItem, equippedValue);
        }

        if (item.ItemList[itemIndex].HealthItem != 0)
        {
            float equippedValue = GetEquippedValue(item.ItemList[itemIndex].itemType, "HealthItem");
            health = ColorizeText("Health: ", item.ItemList[itemIndex].HealthItem, equippedValue);
        }

        if (item.ItemList[itemIndex].HealthRegenItem != 0)
        {
            float equippedValue = GetEquippedValue(item.ItemList[itemIndex].itemType, "HealthRegenItem");
            healthRegen = ColorizeText("Health Regen: ", item.ItemList[itemIndex].HealthRegenItem, equippedValue);
        }

        if (item.ItemList[itemIndex].SpeedItem != 0)
        {
            float equippedValue = GetEquippedValue(item.ItemList[itemIndex].itemType, "SpeedItem");
            speed = ColorizeText("Speed: ", item.ItemList[itemIndex].SpeedItem, equippedValue);
        }

        if (item.ItemList[itemIndex].AttackCooldownAccelerator != 0)
        {
            float equippedValue = GetEquippedValue(item.ItemList[itemIndex].itemType, "AttackCooldownAccelerator");
            cooldownAccelerator = ColorizeText("CD Accelerator: ", item.ItemList[itemIndex].AttackCooldownAccelerator, equippedValue);
        }

        if (item.ItemList[itemIndex].DashCooldownItem != 0)
        {
            float equippedValue = GetEquippedValue(item.ItemList[itemIndex].itemType, "DashCooldownItem");
            dashCD = ColorizeText("Dash Cooldown: ", item.ItemList[itemIndex].DashCooldownItem, equippedValue);
        }

        statsText.text = damage + health + healthRegen + speed + cooldownAccelerator + dashCD;
    }



    private float GetEquippedValue(ItemType type, string valueName)
    {
        switch (type)
        {
            case ItemType.Helmet:
                return GetValue(DataManager.instance.itemData.Helmet, DataManager.instance.itemData.CurrentHelmetIndex, valueName);
            case ItemType.Chestplate:
                return GetValue(DataManager.instance.itemData.Chestplate, DataManager.instance.itemData.CurrentChestplateIndex, valueName);
            case ItemType.Boots:
                return GetValue(DataManager.instance.itemData.Boots, DataManager.instance.itemData.CurrentBootsIndex, valueName);
            default:
                return GetValue(DataManager.instance.itemData.MeeleWeapons, DataManager.instance.itemData.CurrentWeaponIndex, valueName);
        }
    }

    private float GetValue(ItemSO item, int index, string valueName)
    {
        if (item == null || index < 0 || index >= item.ItemList.Count)
        {
            return float.NaN; // Retorna NaN se não houver item equipado
        }

        switch (valueName)
        {
            case "DamageItem":
                return item.ItemList[index].DamageItem;
            case "HealthItem":
                return item.ItemList[index].HealthItem;
            case "HealthRegenItem":
                return item.ItemList[index].HealthRegenItem;
            case "SpeedItem":
                return item.ItemList[index].SpeedItem;
            case "AttackCooldownAccelerator":
                return item.ItemList[index].AttackCooldownAccelerator;
            case "DashCooldownItem":
                return item.ItemList[index].DashCooldownItem;
            default:
                return float.NaN;
        }
    }

    private string ColorizeText(string prefix, float itemValue, float equippedItemValue)
    {
        if (float.IsNaN(equippedItemValue))
        {
            return "<color=#00FF00>" + prefix + itemValue.ToString() + "</color>\n"; // Retorna o texto em verde se não houver item equipado
        }
        else
        {
            string colorTag = itemValue == equippedItemValue ? "<color=#FFFFFF>" : (itemValue < equippedItemValue ? "<color=#FF0000>" : "<color=#00FF00>");
            return colorTag + prefix + itemValue.ToString() + "</color>\n";
        }
    }

    public void SelectItem()
    {
        upgradeManager.StartBuyItem(item, itemIndex);
    }
}
