using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public enum EnemyType
{
    Mage, Tank, Archer, Soldier
}

public enum AttackType
{
    Ranged, Meele
}

[System.Serializable]
public class AttackClass
{
    public float AttackCooldown;
    public float Damage;
    public LayerMask TargetLayer;
    public BulletController bullet;
}

[CreateAssetMenu(fileName ="EnemyType")]
public class EnemySO : ScriptableObject
{
    [Header("Types")]
    public EnemyType enemyType;
    public AttackType attackType;

    [Header("Movement")]
    public Vector2 MinMaxSpeed;
    public float StopRange;

    [Header("Health")]
    public float Health;

    [Header("Animation")]
    public AnimatorController AnimatorController;

    [Header("Attack")]
    public List<AttackClass> AttacksList = new List<AttackClass>();
    public bool isBrave = true;
    public Vector2 MinMaxGetAwayTimer;
}
