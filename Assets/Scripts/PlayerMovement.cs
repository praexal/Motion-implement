using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    MasterControls inputActions;
    Vector2 moveInput;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float climbSpeed = 3f;
    public float aniClimbSpeed = 1f;
    
    private bool isGrounded;
    public bool isClimbing;
    public bool isHanging;
    public bool sliding;


    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;


    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckRadius = 0.2f;

    Animator animator;
    [SerializeField] private LayerMask slopeLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new MasterControls();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => Jump();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        GroundCheck();
        WallCheck();
        gravityControl();
        Climb();
        AnimationControl();
        Slide();
       
        
    }

    void AnimationControl()
    {

        if (isGrounded)
        {
            animator.SetBool("touchdown", true);
            animator.SetFloat("motion", Mathf.Abs(moveInput.x));
        }
        if (!isGrounded)
        {
            animator.SetBool("touchdown", false);
        }

        if (!isGrounded && !isClimbing)
        {
            if(rb.linearVelocity.y < -0.2f)
            {
                animator.SetBool("falling", true);
            }
        }
       
       
       if (isClimbing)
       {
            animator.SetBool("isClimbing", true);
            animator.SetFloat("climb", aniClimbSpeed * moveInput.y);
       }
       else
       {
            animator.SetBool("isClimbing", false);
       }

       //Jump trigger is in the jump method
      

    }
    void Movement()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
       
        if (Mathf.Abs(moveInput.x) > 0.1f && !isClimbing)
        {
            float targetAngle = moveInput.x > 0 ? 90f : -90f;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Adjust rotation speed
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(5, jumpForce, 0, ForceMode.Impulse);
        }
        if (isClimbing)
        {
            rb.AddForce(6 * moveInput.x, jumpForce+(4*jumpForce*moveInput.y) , 0, ForceMode.Impulse);
           
        }
       
        animator.SetTrigger("jump");
    }
    void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheck.position,-transform.up, groundCheckRadius, groundLayer);
    }
    void WallCheck()
    {
        isClimbing = Physics.Raycast(wallCheck.position, transform.forward, wallCheckRadius, wallLayer);
    }

    void gravityControl()
    {
        if(rb.linearVelocity.y < 0 && !isGrounded)
        {
            rb.AddForce(0, -10f, 0);
        }
        if (isClimbing)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    void Climb()
    {
        if(isClimbing)
        {
            rb.linearVelocity = new Vector2 (moveInput.x * 0, moveInput.y * climbSpeed);
        }

    }

    void LedgeGrab()
    {
        if(rb.linearVelocity.y < 0.1f)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position + Vector3.up * 1.5f) + transform.forward*0.5f;
            Vector3 lineDownEnd = (transform.position + Vector3.up * 0.5f) + transform.forward * 0.5f;
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, groundLayer);
            Debug.DrawLine(lineDownStart, lineDownEnd);
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 linefwdstart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 linefwdend = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(linefwdstart, linefwdend, out fwdHit, groundLayer);
                Debug.DrawLine(linefwdstart, linefwdend);
                if (fwdHit.collider != null)
                {
                    rb.useGravity = false;
                    
                    isHanging = true;

                    Vector3 hangPos = new Vector3(fwdHit.point.x + 0.1f , downHit.point.y+ 1.5f, fwdHit.point.z);

                   

                }
            }
        
        }

    }

    void Slide()
    {
        sliding = Physics.Raycast(groundCheck.position, Vector3.down, 1f, slopeLayer);
        if (sliding)
        {
            animator.SetBool("slide", true);
        }
        else
        {
            animator.SetBool("slide", false);
        }
    }

}
