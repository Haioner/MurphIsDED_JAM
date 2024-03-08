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

    //public PlayerAttacks Clone()
    //{
    //    PlayerAttacks clone = new PlayerAttacks();
    //    clone.AttackName = this.AttackName;
    //    clone.AttackDescription = this.AttackDescription;
    //    clone.AttackLevel = this.AttackLevel;
    //    clone.AttackIcon = this.AttackIcon;
    //    clone.AttackDamage = this.AttackDamage;
    //    clone.AttackRange = this.AttackRange;
    //    clone.AttackCooldown = this.AttackCooldown;
    //    clone.AttackMovementSpeed = this.AttackMovementSpeed;
    //    clone.TargetsCount = this.TargetsCount;
    //    return clone;
    //}
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
    //public List<PlayerAttacks> Attacks = new List<PlayerAttacks>();
    public List<AttackSO> Attacks = new List<AttackSO>();
    public List<PlayerAttacks> EqquipedAttacks = new List<PlayerAttacks>();
    //public List<AttackSO> EqquipedAttacks = new List<AttackSO>();

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

    //public void EquipAttack(AttackSO attack, int attackIndex)
    //{
    //    if (attackIndex >= 0 && attackIndex < EqquipedAttacks.Count)
    //    {
    //        AttackSO newEquippedAttack = CloneAttackSO(attack);
    //        EqquipedAttacks[attackIndex] = newEquippedAttack;
    //    }
    //    else
    //        EqquipedAttacks.Add(null);
    //}

    //private AttackSO CloneAttackSO(AttackSO original)
    //{
    //    AttackSO clone = ScriptableObject.CreateInstance<AttackSO>();
    //    clone.AttackName = original.AttackName;
    //    clone.AttackDescription = original.AttackDescription;
    //    clone.AttackLevel = original.AttackLevel;
    //    clone.MaxAttackLevel = original.MaxAttackLevel;
    //    clone.AttackIcon = original.AttackIcon;
    //    clone.AttackDamage = original.AttackDamage;
    //    clone.AttackRange = original.AttackRange;
    //    clone.AttackCooldown = original.AttackCooldown;
    //    clone.AttackMovementSpeed = original.AttackMovementSpeed;
    //    clone.TargetsCount = original.TargetsCount;
    //    return clone;
    //}
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
