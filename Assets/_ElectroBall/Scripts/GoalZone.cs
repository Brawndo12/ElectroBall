using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [Header("Goal Settings")]
    [Tooltip("Which player scores when the ball enters this goal.")]
    [SerializeField] private int scoringPlayer = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        Vector3 zapOrigin = other.transform.position;
        ElectroBallGameManager.Instance.GoalScored(scoringPlayer, zapOrigin);
    }
}