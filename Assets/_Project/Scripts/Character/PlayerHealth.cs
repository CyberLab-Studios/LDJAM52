using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    float health;

    private void Start()
    {
        health = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Ouch!");
        if (health <= 0)
        {
            Debug.Log("Player Morto");
        }
    }
}
