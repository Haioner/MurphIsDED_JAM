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
    public string AttackAnimName;
    public float AttackCooldown;
    public float Damage;
    public float AttackRange;
    public LayerMask TargetLayer;
    public BulletController bullet;
    public AudioClip attackAudio;
}

[CreateAssetMenu(fileName ="EnemyType")]
public class EnemySO : ScriptableObject
{
    [Header("Types")]
    //public EnemyType enemyType;
    public AttackType attackType;

    [Header("Movement")]
    public Vector2 MinMaxSpeed;

    [Header("Health")]
    public float Health;

    [Header("GFX")]
    public AnimatorController AnimatorController;
    public Sprite HeadGFX;
    public Sprite BodyGFX;
    public Sprite ArmGFX;
    public Sprite LegGFX;
    public Sprite weaponGFX;

    [Header("Attack")]
    public List<AttackClass> AttacksList = new List<AttackClass>();
    public bool isBrave = true;
    public Vector2 MinMaxGetAwayTimer;
}
