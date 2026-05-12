using System.Collections;
using UnityEngine;

public class ElectroBallGameManager : MonoBehaviour
{
    public static ElectroBallGameManager Instance { get; private set; }

    [Header("Scene References")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private Rigidbody2D ballRb;

    [Header("Spawn Points")]
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;
    [SerializeField] private Transform ballSpawn;

    [Header("Scoring")]
    [SerializeField] private int player1Score;
    [SerializeField] private int player2Score;

    [Header("Round Timing")]
    [SerializeField] private float resetDelay = 1.25f;
    [SerializeField] private float goalSlowMotionScale = 0.25f;
    [SerializeField] private float goalSlowMotionDuration = 0.6f;

    private bool roundResetting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void GoalScored(int scoringPlayer)
    {
        if (roundResetting) return;

        if (scoringPlayer == 1)
            player1Score++;
        else
            player2Score++;

        Debug.Log($"Player 1: {player1Score} | Player 2: {player2Score}");

        StartCoroutine(GoalSequence());
    }

    private IEnumerator GoalSequence()
    {
        roundResetting = true;

        Time.timeScale = goalSlowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(goalSlowMotionDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        yield return new WaitForSeconds(resetDelay);

        ResetRound();

        roundResetting = false;
    }

    private void ResetRound()
    {
        player1.position = player1Spawn.position;
        player2.position = player2Spawn.position;

        ballRb.position = ballSpawn.position;
        ballRb.velocity = Vector2.zero;
        ballRb.angularVelocity = 0f;
    }
}