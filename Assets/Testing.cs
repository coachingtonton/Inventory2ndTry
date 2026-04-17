using UnityEngine;
using UnityEngine.Rendering;

public class Testing : MonoBehaviour 
{
    public bool isGravityFlipped;

    public Transform wallCheckRight;
    public Transform wallCheckLeft;

    public float wallJumpLockTime = 0.15f;
    private float wallJumpLockTimer;

    public float wallSlideSpeed = -2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float jumpForce = 10f;
    public float gravityScale;
    public float fallMultiplier = 2.5f;

    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;

    public float wallDetachTime = 0.15f;
    public float wallDetachTimer;


    public float airControl;
    public float airDrag;

    public float maxSpeed = 10f;
    public float acceleration = 50f;
    public float decelleration = 30f;

    public bool holdingIntoWall;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (wallDetachTimer > 0)
        {
            
        }


        float moveInput = InputManager.Instance.moveInput;

        bool wallRight = Physics2D.OverlapCircle(wallCheckRight.position, 0.15f, groundLayer);
        bool wallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, 0.15f, groundLayer);
        bool isTouchingWall = wallRight || wallLeft;
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);

        if (wallJumpLockTimer > 0)
        {
            wallJumpLockTimer -= Time.deltaTime;
            moveInput = 0f; // ignore player input during lock
        }


        float targetSpeed = moveInput * maxSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        Debug.DrawRay(wallCheckRight.position, Vector2.right * 0.15f, Color.red);
        Debug.DrawRay(wallCheckLeft.position, Vector2.left * 0.15f, Color.blue);

        float rate;
        if (isGrounded)
        {
            rate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decelleration;
        }
        else
        {
            rate = (Mathf.Abs(targetSpeed) > 0.01f) ? airControl : airDrag;

        }

        if (InputManager.Instance.cntrlPressed)
        {
            isGravityFlipped = !isGravityFlipped;
            rb.gravityScale *= -1;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, 1);

        }


        if (Mathf.Abs(moveInput) < 0.1f) moveInput = 0f;

        float force = speedDiff * rate;

        rb.AddForce(Vector2.right * force, ForceMode2D.Force);

        if (InputManager.Instance.jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (rb.linearVelocity.y <= 5f)
        {
            rb.gravityScale = fallMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }


        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(0f,
                Mathf.Max(rb.linearVelocity.y, wallSlideSpeed));
            Debug.Log("wallsiding");

        }


        bool holdingIntoWall = (wallRight && moveInput > 0.1f) || (wallLeft && moveInput < -0.1f);

        if (holdingIntoWall && !isGrounded)
        {
            rb.linearVelocity = new Vector2(0f, 0f);
            rb.gravityScale = 0f;
            Debug.Log("wall cling");
        }

        if (isTouchingWall && !isGrounded && InputManager.Instance.jumpPressed)
        {

            wallJumpLockTimer = wallJumpLockTime;
            float jumpDirection = wallRight ? -1f : 1f;

            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = gravityScale;

            rb.AddForce(new Vector2(jumpDirection * wallJumpForceX, wallJumpForceY), ForceMode2D.Impulse);

            wallDetachTimer = wallDetachTime;
        }


    }
}

