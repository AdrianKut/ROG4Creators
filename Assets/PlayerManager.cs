using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float force = 900f;
    public bool isGrounded = true;

    public GameObject JumpEffect;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Force);
            
            var jumpEffect = Instantiate(JumpEffect, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), Quaternion.identity);
            Destroy(jumpEffect, 1f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJump", false);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isJump", true);
        }

    }




}
