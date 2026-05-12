using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public enum PlayerNumber { Player1, Player2 }

    [Header("Player")]
    [SerializeField] private PlayerNumber playerNumber;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 65f;
    [SerializeField] private float deceleration = 85f;
    [SerializeField] private float airControlMultiplier = 0.7f;

    [Header("Action Dash")]
    [SerializeField] private float dashForce = 13f;
    [SerializeField] private float dashCooldown = 0.35f;
    [SerializeField] private float dashVelocityLimit = 16f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private float horizontalInput;
    private float verticalInput;
    private float nextDashTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();

        if (IsActionPressed() && Time.time >= nextDashTime)
            ActionDash();
    }

    private void FixedUpdate()
    {
        MoveHorizontally();
        ClampDashSpeed();
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

    private void MoveHorizontally()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;

        bool hasInput = Mathf.Abs(horizontalInput) > 0.01f;
        float accelRate = hasInput ? acceleration : deceleration;
        float control = IsGrounded() ? 1f : airControlMultiplier;

        rb.AddForce(Vector2.right * speedDifference * accelRate * control);

        if (Mathf.Abs(rb.velocity.x) > moveSpeed && !IsActionPressed())
        {
            rb.velocity = new Vector2(
                Mathf.Sign(rb.velocity.x) * moveSpeed,
                rb.velocity.y
            );
        }
    }

    private void ActionDash()
    {
        nextDashTime = Time.time + dashCooldown;

        Vector2 dashDirection = new Vector2(horizontalInput, verticalInput);

        if (dashDirection.sqrMagnitude < 0.01f)
            dashDirection = Vector2.up;

        dashDirection.Normalize();

        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
    }

    private void ClampDashSpeed()
    {
        if (rb.velocity.magnitude > dashVelocityLimit)
            rb.velocity = rb.velocity.normalized * dashVelocityLimit;
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private bool IsActionPressed()
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
        if (groundCheck == null) return;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}