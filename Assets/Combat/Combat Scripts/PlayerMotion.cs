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
    public bool isAttacking, targetlock;

    [Header("PlayerStats")]
    public float maxHP = 100;
    public float currentHP;
    public float maxStamina = 50;
    public float blockDamage;


    [Header("Camera")]
    [SerializeField] private CinemachineCamera freeLookCamera;

    public GameObject enemyObj;

    void Awake()
    {
        controls = new MasterControls();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
        controls.Player.Attack.performed += ctx => Attack();
        controls.Player.Look.performed += ctx => TargetLock();
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
        controls.Player.Attack.performed += ctx => Attack();
        controls.Player.Look.performed += ctx => TargetLock();

    }
    private void Update()
    {
        Movement();
        Block();
        StateController();
        CameraControl();
    }

    void StateController()
    {
        if (attacktime + 0.8f < Time.time)
        {
            hitcount = 0;
            isAttacking = false;
        }
    }

    void Movement()
    {
        animator.SetFloat("motionY", moveInput.y);
        animator.SetFloat("motionX", moveInput.x);
    }
    void Block()
    {
        if (block)
        {
            animator.SetBool("block", true);
            blockDamage = 0.1f;
        }
        else
        {
            animator.SetBool("block", false);
            blockDamage = 1;
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
        }
        if (hitcount == 1 && (attacktime + attackbuffer < Time.time))
        {
            animator.SetTrigger("hit2");
            hitcount = 0;
        }

    }
    void Die()
    {
        animator.SetTrigger("Die");

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage * blockDamage;

        if (currentHP <= 0)
        {
            Die();
        }
    }
    void CameraControl()
    {
        if (!targetlock)
        {
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
        else
        {
            Vector3 camfwd = enemyObj.transform.position - transform.position;
            camfwd.y = 0;
            Vector3 moveDir = camfwd.normalized;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = targetRot;
        }

    }

    void TargetLock()
    {
        if (!targetlock)
        {
            targetlock = true;
        }
        else
        {
            targetlock = false;
        }
    }
}
