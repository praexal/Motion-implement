using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMotion : MonoBehaviour
{
    MasterControls controls;
    Animator animator;
    Vector2 moveInput;
    bool block;

    public float hitcount = 0;
    float attacktime = 0;
    float attackbuffer = 0.2f;
    float timer;
    public bool isAttacking;

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


    [Header("Camera")]
    [SerializeField] private CinemachineCamera freeLookCamera;


    void Awake()
    {
        controls = new MasterControls();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
        controls.Player.Attack.performed += ctx => Attack();
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
        controls.Player.Attack.performed += ctx => Attack();

    }
    private void Update()
    {
        Movement();
        Block();
        StateController();
    }

    void StateController()
    {
        if(attacktime +2f < Time.time)
        {
            isAttacking = false;
        }
    }
    
    void Movement()
    {
        animator.SetFloat("motionY", moveInput.y);
        animator.SetFloat("motionX", moveInput.x);

        if (moveInput.magnitude > 0.1f && freeLookCamera != null)
        {
            // Get camera forward and right vectors (ignoring Y axis)
            Vector3 cameraForward = freeLookCamera.transform.forward;
            Vector3 cameraRight = freeLookCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate movement direction relative to camera
            Vector3 moveDirection = (cameraForward).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = targetRotation;

        }
    }
    void Block()
    {
        if (block)
        {
            animator.SetBool("block", true);
            blockDamage = 5;
            blockCost = 5;
        }
        else
        {
            animator.SetBool("block", false);
            blockDamage = 0;
            blockCost = 0;
        }
    }
    void Attack()
    {

        if (hitcount == 0)
        {
            isAttacking = true;
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
    void Die()
    {
        animator.SetTrigger("Die");
    }

    void TakeDamage()
    {
        currentHP -= damage + blockDamage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

}
