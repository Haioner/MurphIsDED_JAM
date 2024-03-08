using UnityEngine;

[CreateAssetMenu(fileName ="Attacks/Attack")]
public class AttackSO : ScriptableObject
{
    [Header("Attack Info")]
    public int AttackIndex;
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
}
