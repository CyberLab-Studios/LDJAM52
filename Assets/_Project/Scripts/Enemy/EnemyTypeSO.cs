using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Type", menuName = "Scriptable Objects/New enemy")]
public class EnemyTypeSO : ScriptableObject
{
    public float SeeingDistance;
    [Range(0, 360f)]
    public float SeeingAngle;
    public float AttackRange;
    public float AttackCooldown;
    public float Damage;
    public float MaxHealth;
    public float MovementSpeed;
}
