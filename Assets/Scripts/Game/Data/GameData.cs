using Febucci.UI;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttacks
{
    [Header("Attack Info")]
    public int AttackIndex;
    public string AttackName;
    public string AttackDescription;
    public int AttackLevel = 1;
    public int MaxAttackLevel = 15;

    [Header("Attack Stats")]
    public float AttackDamage;
    public float AttackRange;
    public float AttackCooldown;
    public float AttackMovementSpeed;
    public int TargetsCount = 1;
}

[System.Serializable]
public class BasePlayerStats
{
    [Header("Base Player Stats")]
    public float baseHealth = 5;
    public float baseHPRegen;
    public float baseSpeed = 5;
    public float baseDMGmultiplier = 1;
    public float baseCDSpeed = 1;
    public float baseDashCD;
}

[System.Serializable]
public class GameData
{
    [Header("Game")]
    public int MaxGameLevel;
    public int CurrentGameLevel;
    public WaveList waveList;

    [Header("Player Base Stats")]
    public BasePlayerStats baseStats;

    [Header("Player Stats")]
    public int Attack = 0;
    public float Health = 100;
    public float HealthRegen = 0;
    public float Speed = 5;
    public float DamageMultiplier = 1;
    public float AttackCooldownSpeed = 1;

    [Header("Dash")]
    public float DashForce = 10f;
    public float DashCooldown = 2f;

    [Header("Attacks")]
    public List<AttackSO> Attacks = new List<AttackSO>();
    public List<PlayerAttacks> EqquipedAttacks = new List<PlayerAttacks>();

    #region Stats
    public void UpdateStats(ItemData itemData)
    {
        ResetStats();
        //Update new
        UpdateStatsForItemType(itemData.MeeleWeapons, itemData.CurrentWeaponIndex);
        UpdateStatsForItemType(itemData.Helmet, itemData.CurrentHelmetIndex);
        UpdateStatsForItemType(itemData.Chestplate, itemData.CurrentChestplateIndex);
        UpdateStatsForItemType(itemData.Boots, itemData.CurrentBootsIndex);
    }

    private void UpdateStatsForItemType(ItemSO item, int currentIndex)
    {
        if (currentIndex >= 0 && currentIndex < item.ItemList.Count)
        {
            var equippedItem = item.ItemList[currentIndex];
            Health += equippedItem.HealthItem;
            HealthRegen += equippedItem.HealthRegenItem;
            Speed += equippedItem.SpeedItem;
            DamageMultiplier += equippedItem.DamageItem;
            DashCooldown -= equippedItem.DashCooldownItem;
            AttackCooldownSpeed += equippedItem.AttackCooldownAccelerator;
        }
    }

    private void ResetStats()
    {
        Health = baseStats.baseHealth;
        HealthRegen = baseStats.baseHPRegen;
        Speed = baseStats.baseSpeed;
        DamageMultiplier = baseStats.baseDMGmultiplier;
        AttackCooldownSpeed = baseStats.baseCDSpeed;
        DashCooldown = baseStats.baseDashCD;
    }

    #endregion

    #region Attacks
    public void NextAttack(int nextAttack)
    {
        Attack = nextAttack;
    }

    public void UpgradeAttack(int slotIndex)
    {
        EqquipedAttacks[slotIndex].AttackLevel++;
        EqquipedAttacks[slotIndex].AttackDamage += 0.1f;

        if (EqquipedAttacks[slotIndex].AttackLevel % 2 == 0 && EqquipedAttacks[slotIndex].AttackCooldown > 0.15f)
            EqquipedAttacks[slotIndex].AttackCooldown -= 0.05f;

        if (EqquipedAttacks[slotIndex].AttackLevel % 5 == 0)
            EqquipedAttacks[slotIndex].AttackRange += 0.1f;

        if (EqquipedAttacks[slotIndex].AttackLevel % 10 == 0)
            EqquipedAttacks[slotIndex].TargetsCount++;
    }

    public void ConvertAndEquipAttack(AttackSO attack, int attackSlot)
    {
        PlayerAttacks newAttack = new PlayerAttacks();
        newAttack.AttackIndex = attack.AttackIndex;
        newAttack.AttackName = attack.AttackName;
        newAttack.AttackDescription = attack.AttackDescription;
        newAttack.AttackLevel = attack.AttackLevel;
        newAttack.AttackDamage = attack.AttackDamage;
        newAttack.AttackRange = attack.AttackRange;
        newAttack.AttackCooldown = attack.AttackCooldown;
        newAttack.AttackMovementSpeed = attack.AttackMovementSpeed;
        newAttack.TargetsCount = attack.TargetsCount;

        EqquipedAttacks[attackSlot] = newAttack;
    }

    #endregion

    #region Wave
    public void NextGameLevel()
    {
        if(CurrentGameLevel  < MaxGameLevel)
        {
            CurrentGameLevel++;
        }
    }

    public void UnlockNextGameLevel()
    {
        if(CurrentGameLevel >= MaxGameLevel && MaxGameLevel < waveList.waveList.Count - 1)
        {
            MaxGameLevel++;
            DataManager.instance.itemData.dotPoints++;
        }
        DataManager.instance.SaveData();
    }
    #endregion
}
