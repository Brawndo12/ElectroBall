using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("Score Text")]
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2ScoreText;

    [Header("Goal Message")]
    [SerializeField] private TMP_Text goalMessageText;
    [SerializeField] private float goalMessageDuration = 0.8f;

    private Coroutine goalMessageRoutine;

    private void Awake()
    {
        if (goalMessageText != null)
            goalMessageText.gameObject.SetActive(false);
    }

    public void UpdateScore(int player1Score, int player2Score)
    {
        if (player1ScoreText != null)
            player1ScoreText.text = player1Score.ToString();

        if (player2ScoreText != null)
            player2ScoreText.text = player2Score.ToString();
    }

    public void ShowGoalMessage(int scoringPlayer)
    {
        if (goalMessageText == null) return;

        if (goalMessageRoutine != null)
            StopCoroutine(goalMessageRoutine);

        goalMessageRoutine = StartCoroutine(GoalMessageRoutine(scoringPlayer));
    }

    private IEnumerator GoalMessageRoutine(int scoringPlayer)
    {
        goalMessageText.text = $"PLAYER {scoringPlayer} SCORES!";
        goalMessageText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(goalMessageDuration);

        goalMessageText.gameObject.SetActive(false);
    }
}