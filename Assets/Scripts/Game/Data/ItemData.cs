using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [Header("DOTPoints")]
    public int dotPoints;

    [Header("Meele Weapon")]
    public ItemSO MeeleWeapons;
    public int CurrentWeaponIndex;

    [Header("Helmet")]
    public ItemSO Helmet;
    public int CurrentHelmetIndex;

    [Header("ChestPlate")]
    public ItemSO Chestplate;
    public int CurrentChestplateIndex;

    [Header("Boots")]
    public ItemSO Boots;
    public int CurrentBootsIndex;

    public void BuyItem(int itemValue)
    {
        if(dotPoints >= itemValue)
        {
            dotPoints -= itemValue;
        }
    }
}
