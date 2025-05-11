using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    public float speed = 4f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;

    public float chaseRange = 10f;
    public float attackRange = 1f;
    public int damage = 2;
    public float attackCooldown = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    private bool isDead = false;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private float attackTimer = 0f;
    private float footstepTimer = 0f;
    private float footstepInterval = 0.3f;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;
    [SerializeField] private AudioSource footstepSoundEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        if (health <= 0)
        {
            Die();
            return;
        }

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            FollowPlayer();
            HandleFootstepSounds();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
        }

        attackTimer -= Time.deltaTime;
    }

    void HandleFootstepSounds()
    {
        if (animator.GetBool("IsRunning"))
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                footstepSoundEffect.pitch = Random.Range(1.3f, 1.5f);
                footstepSoundEffect.Play();
                footstepTimer = footstepInterval;
            }
        }
    }

    void FollowPlayer()
    {
        animator.SetBool("IsRunning", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Attack()
    {
        if (attackTimer <= 0f)
        {
            animator.SetTrigger("Attack");
            attackSoundEffect.Play();
            
            attackTimer = attackCooldown;

            if (player.TryGetComponent(out PlayerLife playerLife))
            {
                playerLife.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isDead) return;
        
        health -= damage;
        hurtSoundEffect.Play();
        ApplyKnockback(attackerPosition);
    }

    private void ApplyKnockback(Vector2 attackerPosition)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        Vector2 knockbackDirection = ((Vector2)transform.position - attackerPosition).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;
    }

    private void Die()
    {
        if (isDead) return;
        
        health = 0;
        isDead = true;
        animator.SetTrigger("Death");
        animator.SetBool("IsRunning", false);
        deathSoundEffect.Play();

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        Destroy(gameObject, 1f);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            Die();
        }
    }
}