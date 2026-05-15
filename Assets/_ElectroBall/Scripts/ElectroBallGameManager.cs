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

    [Header("UI")]
    [SerializeField] private ScoreUI scoreUI;

    [Header("Effects")]
    [SerializeField] private LightningZapEffect lightningZapEffect;

    [Header("Scoring")]
    [SerializeField] private int player1Score;
    [SerializeField] private int player2Score;

    [Header("Round Timing")]
    [SerializeField] private float resetDelay = 2.5f;
    //[SerializeField] private float goalSlowMotionScale = 0.25f;
    //[SerializeField] private float goalSlowMotionDuration = 0.6f;

    [Header("Settings")]
    [SerializeField] private VisualSettings visualSettings;

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

    private void Start()
    {
        scoreUI?.UpdateScore(player1Score, player2Score);
    }

    public void GoalScored(int scoringPlayer)
    {
        if (roundResetting) return;

        if (scoringPlayer == 1)
            player1Score++;
        else
            player2Score++;

        scoreUI?.UpdateScore(player1Score, player2Score);
        scoreUI?.ShowGoalMessage(scoringPlayer);

        Transform losingPlayer = scoringPlayer == 1 ? player2 : player1;
        lightningZapEffect?.Zap(losingPlayer);

        Debug.Log($"Player 1: {player1Score} | Player 2: {player2Score}");

        StartCoroutine(GoalSequence());
    }

    private IEnumerator GoalSequence()
    {
        roundResetting = true;

        float slowMotionScale = visualSettings != null
            ? visualSettings.goalSlowMotionScale
            : 0.25f;

        float slowMotionDuration = visualSettings != null
            ? visualSettings.goalSlowMotionDuration
            : 1.5f;

        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowMotionDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        yield return new WaitForSecondsRealtime(resetDelay);

        ResetRound();

        roundResetting = false;
    }

    private void ResetRound()
    {
        player1.position = player1Spawn.position;
        player2.position = player2Spawn.position;

        Rigidbody2D player1Rb = player1.GetComponent<Rigidbody2D>();
        Rigidbody2D player2Rb = player2.GetComponent<Rigidbody2D>();

        DestroyAllBubbles();

        player1.position = player1Spawn.position;
        player2.position = player2Spawn.position;

        if (player1Rb != null)
        {
            player1Rb.velocity = Vector2.zero;
            player1Rb.angularVelocity = 0f;
        }

        if (player2Rb != null)
        {
            player2Rb.velocity = Vector2.zero;
            player2Rb.angularVelocity = 0f;
        }

        ballRb.position = ballSpawn.position;
        ballRb.velocity = Vector2.zero;
        ballRb.angularVelocity = 0f;
    }

    private void DestroyAllBubbles()
    {
        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("DashBubble");

        foreach (GameObject bubble in bubbles)
            Destroy(bubble);
    }
}