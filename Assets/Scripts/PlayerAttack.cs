using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack = 0.5f;
    public float attackRange = 0.5f;
    public LayerMask whatIsEnemies;
    public Transform attackPos;
    public int damage = 1;

    private Animator animator;
    private PlayerLife playerLife;
    [SerializeField] private AudioSource attackSoundEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerLife = GetComponent<PlayerLife>();
    }

    private void Update()
    {
        if (playerLife != null && playerLife.isDead) return;

        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetTrigger("Attack");
                attackSoundEffect.Play();

                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                foreach (var enemy in enemiesToDamage)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(damage, transform.position);
                    }
                }

                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
}
