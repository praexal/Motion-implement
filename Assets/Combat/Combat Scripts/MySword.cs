using UnityEngine;

public class MySword : MonoBehaviour
{
    PlayerMotion player;
    private void Start()
    {
        GameObject targetObject = GameObject.FindWithTag("Player");
        player = targetObject.GetComponent<PlayerMotion>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (player.isAttacking)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Sword hit enemy!");

                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(20);
                }
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                 Enemy enemy = other.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(0);
                }
            }

        }
    }
}
