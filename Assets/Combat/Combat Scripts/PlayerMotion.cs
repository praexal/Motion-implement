using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotion : MonoBehaviour
{
    MasterControls controls;
    Animator animator;
    Vector2 moveInput;
    bool block;

    
    
    
    
    
    
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
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => block = true;
        controls.Player.Jump.canceled += ctx => block = false;
    }
    private void Update()
    {
        Movement();
        Block();
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
        }
        else
        {
            animator.SetBool("block", false);
        }
    }

}
