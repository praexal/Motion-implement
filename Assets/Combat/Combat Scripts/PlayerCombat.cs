using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    Animator animator;
    MasterControls controls;
    bool block;

    float hitcount = 0;
    float attacktime = 0;
    float attackbuffer = 0.2f;
    
    [Header("PlayerStats")]
    public float maxHP = 100;
    public float currentHP;
    public float maxStamina = 50;
    public float currentStamina;
    public float staminaRegen;
    public float attackCost;
    public float blockCost;
    public float blockDamage;
    public float damage = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new MasterControls();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
        controls.Player.Attack.performed += ctx => Attack();
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
        controls.Player.Attack.performed += ctx => Attack();
    }
    void Update()
    {
        Block();
    }
    void Attack()
    {

        if (hitcount == 0)
        {
            animator.SetTrigger("hit1");
            hitcount = 1;
            attacktime = Time.time;
            currentStamina -= attackCost;
        }
        if (hitcount == 1 && (attacktime + attackbuffer < Time.time))
        {
            animator.SetTrigger("hit2");
            hitcount = 0;
            currentStamina -= attackCost;
        }
    }
    void TakeDamage()
    {
        currentHP -= damage + blockDamage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Block()
    {
        if (block)
        {
            blockDamage = 5;
            blockCost = 5;
        }
        else
        {
            blockDamage = 0;
            blockCost = 0;
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
    }
}
