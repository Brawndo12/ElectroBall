using UnityEngine;

[CreateAssetMenu(
    fileName = "VisualSettings",
    menuName = "ElectroBall/Visual Settings"
)]
public class VisualSettings : ScriptableObject
{
    [Header("Player 1")]
    public Color player1Color = new Color(0.1f, 1f, 0.25f, 1f);

    [Header("Player 2")]
    public Color player2Color = new Color(1f, 0.1f, 0.9f, 1f);

    [Header("Electricity")]
    public Color electricityColor = new Color(0.4f, 0.9f, 1f, 1f);

    [Header("Goal Slow Motion")]
    [Range(0.05f, 1f)] public float goalSlowMotionScale = 0.25f;
    public float goalSlowMotionDuration = 1.5f;
}