using CyberLabStudios.Game.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    public EnemyTypeSO enemyData;
    public EnemyState actualState = EnemyState.Idle;
    public LayerMask playerLayerMask;
    public LayerMask worldLayerMask;
    public float attackAnimationDelay = 1;

    float health = 10;
    internal bool isSeeingPlayer = false;
    internal Transform playerTarget;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        health = enemyData.MaxHealth;
        agent.speed = enemyData.MovementSpeed;
        attackCooldown = enemyData.AttackCooldown;
        StartCoroutine(CheckFOVRoutine());
    }

    float attackCooldown = 0;
    void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (isSeeingPlayer)
        {
            if (Vector3.Distance(transform.position, playerTarget.position) <= enemyData.AttackRange)
            {
                agent.SetDestination(transform.position);
                if (attackCooldown <= 0 && actualState != EnemyState.Attack)
                {
                    StopAllCoroutines();
                    StartCoroutine(AttackPlayer());
                }
            }
            else
            {
                actualState = EnemyState.Following;
                agent.SetDestination(playerTarget.position);
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        actualState = EnemyState.Attack;
        yield return new WaitForSeconds(attackAnimationDelay);
        if (isSeeingPlayer)
        {
            if (Vector3.Distance(transform.position, playerTarget.position) < enemyData.AttackRange)
            {
                GiveDamage(enemyData.Damage);
                attackCooldown = enemyData.AttackCooldown;
                actualState = EnemyState.Idle;
            }
        }
    }

    public void Die()
    {
        Debug.Log("MORTO!");
        Destroy(gameObject);
    }

    public void GiveDamage(float damage)
    {
        if (playerTarget.TryGetComponent(out PlayerHealth pHealth))
        {
            pHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        agent.SetDestination(playerTarget.position);
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator CheckFOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            CheckFOV();
        }
    }

    void CheckFOV()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, enemyData.SeeingDistance, playerLayerMask);

        if (players.Length != 0)
        {
            playerTarget = players[0].transform;

            Vector3 dirToTarget = (playerTarget.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < enemyData.SeeingAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, worldLayerMask))
                {
                    isSeeingPlayer = true;
                    return;
                }
            }
            isSeeingPlayer = false;
        }
        else if (isSeeingPlayer)
        {
            isSeeingPlayer = false;
            playerTarget = null;
        }
    }
}
public enum EnemyState
{
    Idle, Patrolling, Following, Attack
}