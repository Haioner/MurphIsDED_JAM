using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Helmet, Chestplate, Legging, Boots, //Amor
    MeeleWeapon, RangedWeapon //Weapon
}

[System.Serializable]
public class ItemStats
{
    [Header("Item Type")]
    public string ItemName;
    public string itemID;
    public Sprite Icon;
    public ItemType itemType;
    public string Description;

    [Header("Stats")]
    public float DamageItem;
    public float HealthItem;
    public float HealthRegenItem;
    public float SpeedItem;
    public float DashCooldownItem;
}

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public List<ItemStats> ItemList = new List<ItemStats>();
}
