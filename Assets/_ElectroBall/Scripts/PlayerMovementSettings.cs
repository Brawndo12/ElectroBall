using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerMovementSettings",
    menuName = "ElectroBall/Player Movement Settings"
)]
public class PlayerMovementSettings : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float groundAcceleration = 60f;
    public float airAcceleration = 8f;
    public float airNoInputSlowdown = 3f;

    [Header("Dash")]
    public float dashSpeed = 14f;
    public float dashCooldown = 0.1f;
    public float dashDuration = 0.18f;

    [Header("Dash Bubble")]
    public GameObject dashBubblePrefab;
    public float bubbleDiameter = 2.2f;
    public float bubbleGrowthTime = 0.12f;
    public float bubbleLifetime = 1.5f;
    public float bubbleBallImpulse = 5f;

    [Header("Jump")]
    public float jumpMultiplier = 0.75f;

    [Header("Slide")]
    public float slideSpeed = 11f;
    public float slideDuration = 0.28f;

    [Header("Speed Limits")]
    public float maxOverallSpeed = 18f;

    [Header("Ball Lift")]
    public float ballLiftRadius = 1.1f;
    public float ballLiftHorizontalOffset = 0.35f;
    public float ballLiftHorizontalLerp = 0.35f;
    public float ballLiftVelocityMultiplier = 0.9f;

    [Header("Ground Check")]
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
}