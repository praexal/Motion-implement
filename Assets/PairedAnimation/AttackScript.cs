using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;

public class AttackScript : MonoBehaviour
{
    Victim victim;
    public GameObject vic;
    Animator animator;
    MasterControls controls;

    void Awake()
    {
        victim = vic.GetComponent<Victim>();
        animator = GetComponent<Animator>();
        controls = new MasterControls();
    }
    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Attack.performed += ctx => Attack();
        controls.Player.Jump.performed += ctx => Attack2();
    }


    private float zOffset;
    [SerializeField] private float zOffset1 = 2.3f;
    [SerializeField] private float zOffset2 = 0f;
    [SerializeField] private float zOffset3 = 0f;
    Vector3 inFront;
    Vector3 inFrontRot;
    private void Update()
    {   
        
    }
    Ease moveEase = Ease.OutQuad;
    [SerializeField] float moveDuration;
    void Attack()
    {
        zOffset = zOffset1;
        inFront = vic.transform.position + (vic.transform.forward * zOffset);
        inFrontRot = -vic.transform.forward;
        transform.DOMove(inFront, moveDuration).SetEase(moveEase).OnStart(() =>
        {
            animator.SetTrigger("attack");
            victim.AnimationTrigger();
            Quaternion targetRotation = Quaternion.LookRotation(inFrontRot);
            transform.rotation = targetRotation;
        }
        );
        
    }
    
    void Attack2()
    {
        zOffset = zOffset2;
        inFront = vic.transform.position + (vic.transform.forward * zOffset);
        inFrontRot = -vic.transform.forward;
        transform.DOMove(inFront, moveDuration).SetEase(moveEase).OnStart(() =>
        {
            animator.SetTrigger("attack2");
            victim.AnimationTrigger2();
            Quaternion targetRotation = Quaternion.LookRotation(inFrontRot);
            transform.rotation = targetRotation;
        }
        );
    }
}
