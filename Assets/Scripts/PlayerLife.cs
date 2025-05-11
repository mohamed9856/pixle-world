using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public int maxHealth = 10;
    public int health;
    public bool isDead { get; private set; } = false;
    [SerializeField] private AudioSource deathSoundEffect;

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            health = 0;
            Die();
        }
    }

    public void PlayDeathSound()
    {
        if (deathSoundEffect != null && !deathSoundEffect.isPlaying)
        {
            deathSoundEffect.Play();
        }
    }

    private void Die()
    {
        isDead = true;
        int gemsLostOnDeath = ItemCollector.levelGems;
        PlayDeathSound();

        rb.bodyType = RigidbodyType2D.Static;

        if(ScoreCount.Instance != null)
        {
            ScoreCount.Instance.totalGemsCollected -= gemsLostOnDeath;

            if (ScoreCount.Instance.totalGemsCollected < 0)
            {
                ScoreCount.Instance.totalGemsCollected = 0;
            }
        }

        animator.SetTrigger("Death");
        ItemCollector.levelGems = 0;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
