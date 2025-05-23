using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    BoxCollider2D playerCollider;
    public GameController gameController;
    bool isFacingRight = true;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalInput;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing = false;
    bool canDash = true;
    TrailRenderer trailRenderer; 

    [Header("Jumping")]
    public float jumpForce = 5f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool isOnPlatform; // Check if the player is on a platform

    [Header("Gravity")]
    public float baseGravity = 4f;
    public float maxFallSpeed = 12f;
    public float fallSpeedMultiplier = 2f;

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 0.2f;
    bool isWallSliding;

    //wall jumping
    bool isWallJumping;
    float wallJumpDirection;
    public float wallJumpTime = 0.1f;
    float wallJumpTimer;
    public Vector2 wallJumpForce = new Vector2(5f, 10f);

    [Header("Abilities")]
    public bool canDoubleJump = false;
    public bool canWallJump = false;
    public bool canDashAbility = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        playerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead()) return;
        if (isDashing) return; // Skip movement if dashing
        GroundCheck();
        Gravity();
        WallSlide();
        WallJump();

        if (!isWallJumping) {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            Flip();
        }

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
    }
    private bool IsDead()
    {
        // Return true if the player is dead; you can use a health check or other condition
        return GetComponent<PlayerHealth>().currentHealth <= 0;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && canDashAbility)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine() {
        isDashing = true;
        canDash = false;
        rb.linearVelocity = new Vector2(horizontalInput * dashSpeed, rb.linearVelocity.y);
        trailRenderer.enabled = true; // Enable the trail renderer
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        trailRenderer.enabled = false; // Disable the trail renderer
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void Drop(InputAction.CallbackContext context) {
        if (context.performed && isGrounded && isOnPlatform && playerCollider.enabled) {
           StartCoroutine(DisablePlayerCollider(0.25f)); // Disable the collider for 0.5 seconds
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime) { 
        playerCollider.enabled = false; // Disable the collider
        yield return new WaitForSeconds(disableTime); // Wait for the specified time
        playerCollider.enabled = true; // Re-enable the collider
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PassablePlatform"))
        {
            isOnPlatform = true; // Set the flag to true when on a moving platform
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PassablePlatform"))
        {
            isOnPlatform = false; // Set the flag to true when on a moving platform
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (IsDead()) return;
        // Perform the actual jump
        if (context.performed && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
            animator.SetTrigger("jump");
        }
        if (context.performed && jumpsRemaining == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
            animator.SetTrigger("doubleJump");
        }
        // Cut jump short if player released button early (for variable jump height)
        else if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        //wall jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpTimer = 0f; // Reset the wall jump timer
            animator.SetTrigger("jump");
            if (transform.localScale.x != wallJumpDirection)
            { // Flip the player if they are facing the opposite direction
                isFacingRight = !isFacingRight; // Update the facing direction
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); // Cancel the wall jump after a short delay
        }
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded; // Store previous grounded state
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        // Only reset jumps when first landing (not while already grounded)
        if (isGrounded && !wasGrounded)
        {
            jumpsRemaining = maxJumps; // Reset jumps to max when the player lands
        }
    }

    private bool WallCheck() {

        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void Gravity() { 
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed)); 
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void WallSlide() {

        if (!isGrounded  && WallCheck() && horizontalInput != 0) {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump)); // Cancel any previous invocation of CancelWallJump
        }
        else if (wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump() { 
        isWallJumping = false;
    }

    private void Flip() {
        if (isFacingRight && horizontalInput < 0 || !isFacingRight && horizontalInput > 0) { 
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
