using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [Header("Meele Weapon")]
    public ItemSO MeeleWeapons;
    public string CurrentWeaponID;

    [Header("Helmet")]
    public ItemSO Helmet;
    public string CurrentHelmetID;

    [Header("ChestPlate")]
    public ItemSO Chestplate;
    public string CurrentChestplateID;

    [Header("Legging")]
    public ItemSO Legging;
    public string CurrentLeggingID;

    [Header("Boots")]
    public ItemSO Boots;
    public string CurrentBootsID;

}
