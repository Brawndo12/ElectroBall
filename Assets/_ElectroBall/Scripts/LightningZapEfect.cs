using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningZapEffect : MonoBehaviour
{
    [Header("Lightning")]
    [SerializeField] private Transform lightningOrigin;
    [SerializeField] private float jitterAmount = 0.25f;
    [SerializeField] private int segments = 8;
    [SerializeField] private VisualSettings visualSettings;

    private LineRenderer lineRenderer;
    private Coroutine zapRoutine;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = segments + 1;

        if (visualSettings != null)
        {
            lineRenderer.startColor = visualSettings.electricityColor;
            lineRenderer.endColor = visualSettings.electricityColor;
        }
    }

    public void Zap(Vector3 origin, Transform target)
    {
        if (target == null) return;

        if (zapRoutine != null)
            StopCoroutine(zapRoutine);

        zapRoutine = StartCoroutine(ZapRoutine(origin, target));
    }

    private IEnumerator ZapRoutine(Vector3 origin, Transform target)
    {
        lineRenderer.enabled = true;

        float duration = visualSettings != null
            ? visualSettings.electricityDuration
            : 0.35f;

        float timer = 0f;

        while (timer < duration)
        {
            DrawLightning(origin, target.position);

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    private void DrawLightning(Vector3 start, Vector3 end)
    {
        start.z = 0f;
        end.z = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = Vector3.Lerp(start, end, t);

            if (i != 0 && i != segments)
            {
                point.x += Random.Range(-jitterAmount, jitterAmount);
                point.y += Random.Range(-jitterAmount, jitterAmount);
            }

            point.z = 0f;
            lineRenderer.SetPosition(i, point);
        }
    }
}