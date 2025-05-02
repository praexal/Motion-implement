using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class move : MonoBehaviour
{
    MasterControls controls;
    Vector2 moveI;
    Animator animator;
    [SerializeField] private CinemachineCamera freecamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controls = new MasterControls();
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveI = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveI = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed += ctx => moveI = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveI = Vector2.zero;
    }


    void Update()
    {
        Move();
    }
    void Move()
    {
        animator.SetFloat("move", Mathf.Abs(moveI.y));
        animator.SetFloat("movex", Mathf.Abs(moveI.x));

        if(moveI.magnitude > 0.1 && freecamera != null)
        {
            // Get camera forward and right vectors (ignoring Y axis)
            Vector3 cameraForward = freecamera.transform.forward;
            Vector3 cameraRight = freecamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate movement direction relative to camera
            Vector3 moveDirection = (cameraForward * moveI.y + cameraRight*moveI.x).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = targetRotation;
        }
    }
}
