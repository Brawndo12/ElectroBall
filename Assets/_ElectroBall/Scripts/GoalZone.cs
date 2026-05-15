using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [Header("Goal Settings")]
    [Tooltip("Which player scores when the ball enters this goal.")]
    [SerializeField] private int scoringPlayer = 1;

    private Collider2D goalCollider;

    private void Awake()
    {
        goalCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        Vector2 ballPosition = other.transform.position;

        Vector2 zapOrigin = goalCollider != null
            ? goalCollider.ClosestPoint(ballPosition)
            : ballPosition;

        ElectroBallGameManager.Instance.GoalScored(scoringPlayer, zapOrigin);
    }
}