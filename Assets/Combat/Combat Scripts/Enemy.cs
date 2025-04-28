using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyStats")]
    public float maxHP = 100;
    public float currentHP;
    public float maxStamina = 50;
    public float blockDamage;
    public Transform player;
    Animator animator;
    public bool Dead, isAttacking;
    float hitmoment;
    float RNG;
    Vector3 playerPos;
    Vector3 enemyPos;
    float AttackingTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time> hitmoment + 0.02f)
        {
            Time.timeScale = 1f;
        }
        Locomotion();

        if(Time.time > AttackingTime + 1f)
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
            Time.timeScale = 0.1f;


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
        Vector3 playerPos = player.position;
        Vector3 enemyPos = transform.position;
        if (!Dead)
        {
            Vector3 direction = (playerPos - enemyPos);
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            Debug.Log(Mathf.Abs(Vector3.Distance(playerPos, enemyPos)));
            if (Mathf.Abs(Vector3.Distance(playerPos, enemyPos)) > 1.5f)
            {
                animator.SetBool("move", true);
            }
            if (Mathf.Abs(Vector3.Distance(playerPos, enemyPos)) <= 1.5f)
            {
                animator.SetBool("move", false);
                Attack();
            }
        }
    }

    void Attack()
    {
        RandomNumberGenerator();
        if(RNG != 0)
        {
            AttackingTime = Time.time;
            isAttacking = true;
            animator.SetTrigger("hit1");
        }
    }

    void RandomNumberGenerator()
    {
        RNG = UnityEngine.Random.Range(0, 4);
    }
}
