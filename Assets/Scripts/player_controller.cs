using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f; // Horizontal movement speed
    [SerializeField] private float acceleration = 15f; // Speed of acceleration
    [SerializeField] private float deceleration = 20f; // Speed of deceleration
    [SerializeField] private float velocityPower = 0.9f; // Adjusts the influence of velocity
    [SerializeField] private float slipperyFactor = 0.5f; // Makes movement slippery

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f; // Initial jump force
    [SerializeField] private float fallMultiplier = 2.5f; // Gravity multiplier when falling
    [SerializeField] private float lowJumpMultiplier = 2f; // Gravity multiplier for short jumps
    [SerializeField] private float coyoteTime = 0.1f; // Time after leaving ground where you can still jump
    [SerializeField] private float jumpBufferTime = 0.1f; // Time before landing where jump input is remembered
    [SerializeField] private float jumpRandomness = 0.5f; // Randomness added to jump force

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check
    [SerializeField] private Transform groundCheckPoint; // Point to check for ground

    private Rigidbody2D rb;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleGravity();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    /// <summary>
    /// Handles slippery horizontal movement.
    /// </summary>
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Calculate target speed with slippery factor
        float targetSpeed = moveInput * moveSpeed * slipperyFactor;

        // Calculate difference between current and target speed
        float speedDifference = targetSpeed - rb.linearVelocity.x;

        // Apply acceleration or deceleration
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velocityPower) * Mathf.Sign(speedDifference);

        // Apply force
        rb.AddForce(movement * Vector2.right);
    }

    /// <summary>
    /// Handles jumping with randomness.
    /// </summary>
    private void HandleJump()
    {
        // Coyote time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Perform jump with randomness
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            float randomJumpForce = jumpForce + Random.Range(-jumpRandomness, jumpRandomness);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, randomJumpForce);
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            isJumping = true;
        }

        // Cancel jump if button released early
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            isJumping = false;
        }
    }

    /// <summary>
    /// Adjusts gravity for better jump feel.
    /// </summary>
    private void HandleGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Checks if the player is grounded.
    /// </summary>
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
}