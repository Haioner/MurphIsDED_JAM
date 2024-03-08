using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttacks
{
    [Header("Attack Info")]
    public string AttackName;
    public string AttackDescription;
    public int AttackLevel = 1;
    public int MaxAttackLevel = 15;
    public Sprite AttackIcon;

    [Header("Attack Stats")]
    public float AttackDamage;
    public float AttackRange;
    public float AttackCooldown;
    public float AttackMovementSpeed;
    public int TargetsCount = 1;

    public PlayerAttacks Clone()
    {
        PlayerAttacks clone = new PlayerAttacks();
        clone.AttackName = this.AttackName;
        clone.AttackDescription = this.AttackDescription;
        clone.AttackLevel = this.AttackLevel;
        clone.AttackIcon = this.AttackIcon;
        clone.AttackDamage = this.AttackDamage;
        clone.AttackRange = this.AttackRange;
        clone.AttackCooldown = this.AttackCooldown;
        clone.AttackMovementSpeed = this.AttackMovementSpeed;
        clone.TargetsCount = this.TargetsCount;
        return clone;
    }
}

[System.Serializable]
public class GameData
{
    [Header("Game")]
    public int MaxGameLevel;
    public int CurrentGameLevel;
    public WaveList waveList;

    [Header("Player")]
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
    public List<PlayerAttacks> Attacks = new List<PlayerAttacks>();
    public List<PlayerAttacks> EqquipedAttacks = new List<PlayerAttacks>();

    #region Attacks
    public void NextAttack(int nextAttack)
    {
        Attack = nextAttack;
    }

    public void UpgradeAttack(int slotIndex)
    {
        EqquipedAttacks[slotIndex].AttackLevel++;
        EqquipedAttacks[slotIndex].AttackDamage++;

        if (EqquipedAttacks[slotIndex].AttackLevel % 2 == 0 && EqquipedAttacks[slotIndex].AttackCooldown > 0.15f)
            EqquipedAttacks[slotIndex].AttackCooldown -= 0.05f;

        if (EqquipedAttacks[slotIndex].AttackLevel % 5 == 0)
            EqquipedAttacks[slotIndex].AttackRange += 0.1f;

        if (EqquipedAttacks[slotIndex].AttackLevel % 10 == 0)
            EqquipedAttacks[slotIndex].TargetsCount++;
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
        }
        DataManager.instance.SaveData();
    }
    #endregion
}
