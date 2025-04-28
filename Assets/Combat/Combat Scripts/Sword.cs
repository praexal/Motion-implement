using UnityEngine;

public class Sword : MonoBehaviour
{
    PlayerMotion player;
    Enemy enemy;

    private void Start()
    {
        GameObject PlayerObj = GameObject.FindGameObjectWithTag("Player");
        player = PlayerObj.GetComponent<PlayerMotion>();
        GameObject EnemyObj = GameObject.FindGameObjectWithTag("Enemy");
        enemy = EnemyObj.GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider opponent)
    {
        if (player.isAttacking)
        {
            if (opponent.CompareTag("Enemy"))
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(10);
                }
            }
        }
        if (enemy.isAttacking)
        {
            if (opponent.CompareTag("Player"))
            {
                if (player != null)
                {
                    player.TakeDamage(5);
                }
            }
        }
    }

}
