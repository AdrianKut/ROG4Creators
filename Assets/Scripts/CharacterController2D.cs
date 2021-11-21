using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)]
    [SerializeField]
    private float movementSmoothing = .05f; // How much to smooth out the movement

    [SerializeField]
    private float runSpeed = 15f;

    [SerializeField]
    private float jumpSpeed = 15f;

    private Animator animator;
    private Rigidbody2D rb2D;

    private float horizontalMove;
    private float verticalMove;
    private bool facingRight = true; // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;
    private static readonly int Speed = Animator.StringToHash("Speed");
    public bool isOnGround = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * jumpSpeed;

        animator.SetFloat(Speed, Mathf.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        Move(horizontalMove * Time.fixedDeltaTime);
        Jump(verticalMove * Time.fixedDeltaTime);
    }

    private void Move(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb2D.velocity.y);
        // And then smoothing it out and applying it to the character
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Jump(float jump)
    {
        if (isOnGround)
            rb2D.velocity += new Vector2(0, jump);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isOnGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isOnGround = false;
    }

}