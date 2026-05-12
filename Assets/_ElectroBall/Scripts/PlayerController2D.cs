using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public enum PlayerNumber
    {
        Player1,
        Player2
    }

    [Header("Player")]
    [SerializeField] private PlayerNumber playerNumber;

    [Header("Movement")]
    [SerializeField] private float moveForce = 45f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float dragWhenNoInput = 6f;

    [Header("Dash / Kick")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashCooldown = 0.45f;
    [SerializeField] private float dashDuration = 0.12f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.right;
    private float nextDashTime;
    private float dashEndTime;
    private bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();

        if (input.sqrMagnitude > 0.01f)
            lastMoveDirection = input.normalized;

        if (GetActionButtonDown() && Time.time >= nextDashTime)
            Dash();
    }

    private void FixedUpdate()
    {
        rb.AddForce(input * moveForce);

        if (rb.velocity.magnitude > maxSpeed && !isDashing)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        rb.drag = input.sqrMagnitude < 0.01f ? dragWhenNoInput : 0f;
        if (isDashing && Time.time >= dashEndTime)
            isDashing = false;
    }

    private void ReadInput()
    {
        if (playerNumber == PlayerNumber.Player1)
        {
            input = new Vector2(
                GetAxis(KeyCode.A, KeyCode.D),
                GetAxis(KeyCode.S, KeyCode.W)
            );
        }
        else
        {
            input = new Vector2(
                GetAxis(KeyCode.LeftArrow, KeyCode.RightArrow),
                GetAxis(KeyCode.DownArrow, KeyCode.UpArrow)
            );
        }

        input = Vector2.ClampMagnitude(input, 1f);
    }

    private float GetAxis(KeyCode negative, KeyCode positive)
    {
        float value = 0f;

        if (Input.GetKey(negative)) value -= 1f;
        if (Input.GetKey(positive)) value += 1f;

        return value;
    }

    private bool GetActionButtonDown()
    {
        return playerNumber == PlayerNumber.Player1
            ? Input.GetKeyDown(KeyCode.Space)
            : Input.GetKeyDown(KeyCode.RightControl);
    }

    private void Dash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;

        rb.AddForce(lastMoveDirection * dashForce, ForceMode2D.Impulse);
    }
}