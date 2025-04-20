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
    public float basicMultiplier = 1f;
    
    private bool isGrounded;
    public bool isClimbing;
    public bool isHanging;
    public bool sliding;
    RaycastHit downHit;
    RaycastHit fwdHit;


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
    [SerializeField] private float hangXoffset;
    [SerializeField] private float hangYoffset;

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
        LedgeGrab();
        if (isGrounded)
        {
            isHanging = false;
        }

        
       
        
    }

    void AnimationControl()
    {

        if (isGrounded)
        {
            animator.SetBool("touchdown", true);
            animator.SetFloat("motion", Mathf.Abs(moveInput.x));
            animator.SetBool("falling", false);
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
                animator.SetTrigger("fallingT");
            }
        }
       
       
       if (isClimbing)
       {
            animator.SetBool("isClimbing", true);
            animator.SetFloat("climb", 1 * moveInput.y);
       }
       else
       {
            animator.SetBool("isClimbing", false);
       }

        if (isHanging)
        {
            animator.SetBool("hanging", true);
        }
        else
        {
            animator.SetBool("hanging", false);
        }

       //Jump trigger is in the jump method
      

    }
    void Movement()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y);
            isHanging = false;
        }
       
        if (Mathf.Abs(moveInput.x) > 0.1f && !isClimbing)
        {
            float targetAngle;
            if (moveInput.x > 0)
            {
                targetAngle = 90f;  // Facing right
                hangXoffset = hangXoffset;
                basicMultiplier = 1;
            }
            else
            {
                targetAngle = -90f; // Facing left
                hangXoffset = -hangXoffset;
                basicMultiplier = -1;
            }
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
        if (isHanging)
        {
            Vector3 targetpos = new Vector3 (fwdHit.point.x+0.2f, downHit.point.y,fwdHit.point.z);
            transform.position = Vector3.Slerp(transform.position, targetpos, 1f);
            isHanging = false;
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
        if(rb.linearVelocity.y < 0 && !isGrounded && !isHanging )
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
            Vector3 lineDownStart = (transform.position + Vector3.up * 2f) + transform.forward;
            Vector3 lineDownEnd = (transform.position + Vector3.up * 0.5f) + transform.forward;
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, groundLayer);
            Debug.DrawLine(lineDownStart, lineDownEnd);
            if (downHit.collider != null)
            {
                Vector3 linefwdstart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 linefwdend = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(linefwdstart, linefwdend, out fwdHit, groundLayer);
                Debug.DrawLine(linefwdstart, linefwdend);
                if (fwdHit.collider != null)
                {
                    rb.useGravity = false;
                    Debug.Log("I should be hanging");
                    isHanging = true;
                    Vector3 hangPos = new Vector3(fwdHit.point.x +(hangXoffset) , downHit.point.y+ hangYoffset);
                    transform.forward = -fwdHit.normal;
                    transform.position = hangPos;
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
