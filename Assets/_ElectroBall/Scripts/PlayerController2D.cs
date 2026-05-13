using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public enum PlayerNumber { Player1, Player2 }

    [Header("Player")]
    [SerializeField] private PlayerNumber playerNumber;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float groundAcceleration = 60f;
    [SerializeField] private float airAcceleration = 8f;
    [SerializeField] private float airNoInputSlowdown = 3f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float dashDuration = 0.18f;

    [Header("Jump")]
    [SerializeField] private float jumpMultiplier = 0.75f;

    [Header("Slide")]
    [SerializeField] private float slideSpeed = 11f;
    [SerializeField] private float slideDuration = 0.28f;

    [Header("Speed Limits")]
    [SerializeField] private float maxNonDashHorizontalSpeed = 7f;
    [SerializeField] private float maxOverallSpeed = 18f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ball Lift")]
    [SerializeField] private float ballLiftRadius = 1.1f;
    [SerializeField] private Vector2 ballLiftOffset = new Vector2(0.3f, 0.8f);

    private Rigidbody2D rb;

    private float horizontalInput;
    private float verticalInput;

    private float nextActionTime;
    private float dashEndTime;
    private float slideEndTime;

    private int facingDirection = 1;
    private bool wasGrounded;

    private bool IsDashing => Time.time < dashEndTime;
    private bool IsSliding => Time.time < slideEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();
        UpdateFacingDirection();

        bool grounded = IsGrounded();

        if (IsSliding && !grounded && wasGrounded)
            GroundJump();

        if (IsActionPressedThisFrame() && Time.time >= nextActionTime)
            HandleAction(grounded);

        wasGrounded = grounded;
    }

    private void FixedUpdate()
    {
        MoveHorizontally();
        ClampSpeed();
    }

    private void ReadInput()
    {
        if (playerNumber == PlayerNumber.Player1)
        {
            horizontalInput = GetAxis(KeyCode.A, KeyCode.D);
            verticalInput = GetAxis(KeyCode.S, KeyCode.W);
        }
        else
        {
            horizontalInput = GetAxis(KeyCode.LeftArrow, KeyCode.RightArrow);
            verticalInput = GetAxis(KeyCode.DownArrow, KeyCode.UpArrow);
        }
    }

    private void UpdateFacingDirection()
    {
        if (horizontalInput > 0.01f)
            facingDirection = 1;
        else if (horizontalInput < -0.01f)
            facingDirection = -1;
    }

    private void MoveHorizontally()
    {
        if (IsDashing || IsSliding)
            return;

        bool grounded = IsGrounded();
        bool hasInput = Mathf.Abs(horizontalInput) > 0.01f;

        float currentX = rb.velocity.x;
        float targetX = hasInput ? horizontalInput * moveSpeed : 0f;

        float accel = grounded
            ? groundAcceleration
            : hasInput ? airAcceleration : airNoInputSlowdown;

        float newX = Mathf.MoveTowards(
            currentX,
            targetX,
            accel * Time.fixedDeltaTime
        );

        rb.velocity = new Vector2(newX, rb.velocity.y);
    }

    private void HandleAction(bool grounded)
    {
        nextActionTime = Time.time + dashCooldown;

        if (IsSliding)
        {
            GroundJump();
            return;
        }

        if (grounded)
        {
            if (verticalInput < -0.01f)
                StartSlide();
            else
                GroundJump();

            return;
        }

        AirDash();
    }

    private void GroundJump()
    {
        slideEndTime = 0f;

        float jumpSpeed = dashSpeed * jumpMultiplier;

        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

        TryLiftBall(jumpSpeed);
    }

    private void StartSlide()
    {
        slideEndTime = Time.time + slideDuration;

        int direction = GetHorizontalActionDirection();

        rb.velocity = new Vector2(direction * slideSpeed, rb.velocity.y);
    }

    private void AirDash()
    {
        Vector2 dashDirection = new Vector2(horizontalInput, verticalInput);

        if (dashDirection.sqrMagnitude < 0.01f)
            dashDirection = Vector2.up;

        dashDirection.Normalize();

        dashEndTime = Time.time + dashDuration;

        rb.velocity = dashDirection * dashSpeed;
    }

    private int GetHorizontalActionDirection()
    {
        if (horizontalInput > 0.01f)
            return 1;

        if (horizontalInput < -0.01f)
            return -1;

        return facingDirection;
    }

    private void TryLiftBall(float jumpSpeed)
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");

        if (ball == null)
            return;

        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();

        if (ballRb == null)
            return;

        float distance = Vector2.Distance(rb.position, ballRb.position);

        if (distance > ballLiftRadius)
            return;

        Vector2 offset = new Vector2(
            ballLiftOffset.x * facingDirection,
            ballLiftOffset.y
        );

        ballRb.position = rb.position + offset;

        ballRb.velocity = new Vector2(
            rb.velocity.x,
            jumpSpeed * 0.9f
        );
    }

    private void ClampSpeed()
    {
        if (rb.velocity.magnitude > maxOverallSpeed)
            rb.velocity = rb.velocity.normalized * maxOverallSpeed;
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private bool IsActionPressedThisFrame()
    {
        return playerNumber == PlayerNumber.Player1
            ? Input.GetKeyDown(KeyCode.Space)
            : Input.GetKeyDown(KeyCode.RightControl);
    }

    private float GetAxis(KeyCode negative, KeyCode positive)
    {
        float value = 0f;

        if (Input.GetKey(negative)) value -= 1f;
        if (Input.GetKey(positive)) value += 1f;

        return value;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );

        Gizmos.DrawWireSphere(transform.position, ballLiftRadius);
    }
}