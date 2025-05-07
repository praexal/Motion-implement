using UnityEngine;

public class Victim : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   public void AnimationTrigger()
    {
        animator.SetTrigger("die");
    }
    public void AnimationTrigger2()
    {
        animator.SetTrigger("die2");
    }
}
