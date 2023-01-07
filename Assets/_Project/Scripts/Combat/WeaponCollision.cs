using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public float damage = 10;
    bool isAttacking = false;

    private void Start()
    {
        GameEvents.Instance.onEquipScythe += GetScytheData;
        GameEvents.Instance.onAttack += OnAttack;
    }


    private void OnDisable()
    {
        GameEvents.Instance.onEquipScythe -= GetScytheData;
        GameEvents.Instance.onAttack -= OnAttack;
    }

    private void OnAttack(float time)
    {
        StopAllCoroutines();
        StartCoroutine(ResetAttackTrigger(time));
    }

    IEnumerator ResetAttackTrigger(float time)
    {
        isAttacking = true;
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }

    private void GetScytheData(ScytheData obj)
    {
        damage = obj.Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            Debug.Log($"Hit with: {other.gameObject.name}");
        }
    }
}
