using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttacks
{
    [Header("Attack Stats")]
    public string AttackName;
    public float AttackDamage;
    public float AttackRange;
    public float AttackCooldown;
    public float AttackMovementSpeed;
    public int TargetsCount = 1;

    [Header("Attack State")]
    public bool AttackEquipped;
    public bool AttackUnlocked;
}

[System.Serializable]
public class GameData
{
    [Header("Player")]
    public int Attack = 0;
    public float Health = 100;
    public float Speed = 5;

    [Header("Level")]
    public int Level = 1;
    public float CurrentXP = 0;
    public float MaxXP = 10;
    public int XpToAdd = 1;

    [Header("Habilities")]
    public float DashForce = 10f;
    public float DashCooldown = 2f;

    [Header("Attacks")]
    public List<PlayerAttacks> Attacks = new List<PlayerAttacks>();

    #region Level Methods

    public void AddXP()
    {
        CurrentXP += XpToAdd;
    }

    #endregion

    public void NextAttack(int nextAttack)
    {
        Attack = nextAttack;
    }
}
