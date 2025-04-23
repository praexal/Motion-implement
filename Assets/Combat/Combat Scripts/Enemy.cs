using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("PlayerStats")]
    public float maxHP = 100;
    public float currentHP;
    public float maxStamina = 50;
    public float currentStamina;
    public float staminaRegen;
    public float attackCost;
    public float blockCost;
    public float blockDamage;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage + blockDamage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        float timer = 0 + Time.deltaTime;
        if (timer > 2.5)
        {
            animator.enabled = false;

        }
    }
}
