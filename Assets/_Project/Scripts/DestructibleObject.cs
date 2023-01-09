using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleObject : MonoBehaviour, IEnemy
{
    public float health;
    public UnityEvent onDie;


    void Update()
    {
        if (health < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        onDie?.Invoke();
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
