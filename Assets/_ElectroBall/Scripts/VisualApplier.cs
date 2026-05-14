using UnityEngine;

public class VisualApplier : MonoBehaviour
{
    [SerializeField] private PlayerController2D.PlayerNumber playerNumber;
    [SerializeField] private VisualSettings visualSettings;

    [Header("Renderers")]
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private SpriteRenderer goalRenderer;

    public Color PlayerColor
    {
        get
        {
            if (visualSettings == null)
                return Color.white;

            return playerNumber == PlayerController2D.PlayerNumber.Player1
                ? visualSettings.player1Color
                : visualSettings.player2Color;
        }
    }

    private void Awake()
    {
        ApplyColors();
    }

    private void OnValidate()
    {
        ApplyColors();
    }

    private void ApplyColors()
    {
        Color color = PlayerColor;

        if (playerRenderer != null)
            playerRenderer.color = color;

        if (goalRenderer != null)
            goalRenderer.color = color;
    }
}