using System.Collections;
using UnityEngine;

/// <summary>
/// The character controller!
/// </summary>
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.5f;

    [SerializeField]
    private float jumpSpeed = 1.0f;

    [SerializeField]
    private float impulseStrength = 2.0f;

    [SerializeField]
    private float bounceAngle = 35.0f;

    [SerializeField]
    private LayerMask floorMask;

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private ParticleSystem jumpParticles;

    private sbyte direction = 1;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private AudioSource audio;

    private void Awake()
    {
        AddListeners();
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void AddListeners()
    {
        InputManager.Instance.OnTap += Jump;
    }

    private void RemoveListeners()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTap -= Jump;
        }
    }

    /// <summary>
    /// Simply... moves the player
    /// </summary>
    private void Move()
    {
        if (GameManager.Instance.GameRunning)
        {
            rigidBody.velocity = new Vector2(direction * moveSpeed, rigidBody.velocity.y);
        }
    }

    /// <summary>
    /// Makes the character jump
    /// </summary>
    private void Jump()
    {
        if (GameManager.Instance.GameRunning)
        {
            if (IsGrounded())
            {
                rigidBody.velocity = Vector2.up * jumpSpeed;

                jumpParticles.Play();
                audio.Play();
            }
            else if (IsInWall())
            {
                jumpParticles.Play();
                direction *= -1;
                rigidBody.velocity = Vector2.up * jumpSpeed;
                var angleVector = new Vector2(Mathf.Cos(bounceAngle * Mathf.Deg2Rad) * direction, Mathf.Sin(bounceAngle * Mathf.Deg2Rad));
                rigidBody.AddForce(angleVector * impulseStrength, ForceMode2D.Impulse);

                animator.SetBool("IsRight", direction == 1? true : false);
                audio.Play();
            } 
        }
    }

    /// <summary>
    /// Checks if character is grounded
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        var hit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0.0f, Vector2.down, 0.01f, floorMask);

        return hit2D.collider != null && !hit2D.collider.CompareTag("Player");
    }

    /// <summary>
    /// Checks if character is in contact with any wall
    /// </summary>
    /// <returns></returns>
    private bool IsInWall()
    {
        var hitLeft2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0.0f, Vector2.left, 0.02f, wallMask);
        var hitRightt2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0.0f, Vector2.right, 0.02f, wallMask);

        return hitLeft2D.collider != null || hitRightt2D.collider != null;
    }
}
