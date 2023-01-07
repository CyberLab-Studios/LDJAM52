using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New scythe", menuName = "Scriptable Objects/Scythe Data")]
public class ScytheData : ScriptableObject
{
    public float Damage;
    public float Cooldown;
    public GameObject ScythePrefab;
}
