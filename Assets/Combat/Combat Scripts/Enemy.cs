using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("EnemyStats")]
    public float maxHP = 100;
    public float currentHP;
    public float maxStamina = 50;
    public float blockDamage, rootmotionfix;
    public Transform player;
    Animator animator;
    public bool Dead, isAttacking;
    float hitmoment;
    float RNG;
    float AttackingTime;
    NavMeshAgent agent;
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > hitmoment + 0.01f)
        {
            Time.timeScale = 1f;
        }
        Locomotion();

        if (Time.time > AttackingTime + 1f)
        {
            isAttacking = false;
        }
    }
    public void TakeDamage(int damage)
    {
        if (!Dead)
        {
            currentHP -= damage + blockDamage;
            hitmoment = Time.time;
            Time.timeScale = 0.2f;


            if (currentHP <= 0)
            {
                Die();
                Dead = true;
            }
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
    }

    void Locomotion()
    {
        if (!Dead)
        {
            agent.SetDestination(player.position);
        }
        if (agent.velocity.magnitude != 0 )
        {
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
    }

    void Attack()
    {
        RandomNumberGenerator();
        if (RNG != 0)
        {
            AttackingTime = Time.time;
            isAttacking = true;
            animator.SetTrigger("hit1");
        }
    }
    private void OnAnimatorMove()
    {
        if (animator.GetBool("move"))
        {
            agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }
    void RandomNumberGenerator()
    {
        RNG = UnityEngine.Random.Range(0, 4);
    }
}
