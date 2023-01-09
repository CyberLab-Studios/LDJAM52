using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillableObject : MonoBehaviour, IEnemy
{
    public float health;
    public UnityEvent onDie;
    bool isDied = false;

    void Update()
    {
        if (health < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!isDied)
        {
            onDie?.Invoke();
            isDied = true;
        }
    }

    public void GiveDamage(float damage)
    {
        //Cannot damage player
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
