using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] public PlayerLife playerHealth;
    
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
