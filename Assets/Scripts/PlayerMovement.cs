using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private SpriteRenderer sprite;
    private Animator animator;
    private PlayerLife playerLife;

    private float dirx = 0f;
    private bool wasGrounded;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idle, jumping, running, falling, combat }

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource walkSoundEffect;
    [SerializeField] private AudioSource landSoundEffect;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerLife = GetComponent<PlayerLife>();
        wasGrounded = IsGrounded();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (playerLife != null && playerLife.isDead) return;

        dirx = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(dirx * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        UpdateAnimationState();
        CheckLanding();
        wasGrounded = IsGrounded();
    }

    

    private void CheckLanding()
    {
        bool isGrounded = IsGrounded();
        if (!wasGrounded && isGrounded)
        {
            landSoundEffect.Play();
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirx != 0f)
        {
            state = MovementState.running;
            if (IsGrounded() && !walkSoundEffect.isPlaying)
            {
                walkSoundEffect.Play();
            }
        }
        else
        {
            state = MovementState.idle;
            if (walkSoundEffect.isPlaying)
            {
                walkSoundEffect.Stop();
            }
        }

        if (rb.linearVelocity.y > 0.1f)
        {
            state = MovementState.jumping;
            if (walkSoundEffect.isPlaying)
            {
                walkSoundEffect.Stop();
            }
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        if (dirx > 0f) sprite.flipX = true;
        else if (dirx < 0f) sprite.flipX = false;

        animator.SetInteger("AnimState", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}