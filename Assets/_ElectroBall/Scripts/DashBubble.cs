using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DashBubble : MonoBehaviour
{
    private CircleCollider2D bubbleCollider;
    private SpriteRenderer spriteRenderer;

    private float targetDiameter;
    private float growthTime;
    private float lifetime;
    private float ballImpulse;
    private float fadeTime;

    private float timer;
    private float fadeTimer;

    private bool isFading;

    public void Initialize(
        float diameter,
        float growTime,
        float existTime,
        float impulse,
        float popFadeTime,
        Color bubbleColor
        )
    {
        targetDiameter = diameter;
        growthTime = growTime;
        lifetime = existTime;
        ballImpulse = impulse;
        fadeTime = popFadeTime;

        bubbleCollider = GetComponent<CircleCollider2D>();
        bubbleCollider.isTrigger = true;
        bubbleCollider.radius = 0.5f;
        bubbleCollider.enabled = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = bubbleColor;

        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (isFading)
        {
            UpdateFade();
            return;
        }

        timer += Time.deltaTime;

        float growPercent = growthTime <= 0f
            ? 1f
            : Mathf.Clamp01(timer / growthTime);

        transform.localScale = Vector3.one * (targetDiameter * growPercent);

        if (timer >= lifetime)
            BeginFade();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFading) return;
        if (!other.CompareTag("Ball")) return;

        Rigidbody2D ballRb = other.attachedRigidbody;
        if (ballRb == null) return;

        Vector2 direction = (ballRb.position - (Vector2)transform.position).normalized;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.up;

        ballRb.AddForce(direction * ballImpulse, ForceMode2D.Impulse);

        BeginFade();
    }

    public void BeginFade()
    {
        if (isFading) return;

        isFading = true;
        fadeTimer = 0f;

        if (bubbleCollider != null)
            bubbleCollider.enabled = false;
    }

    private void UpdateFade()
    {
        fadeTimer += Time.deltaTime;

        float fadePercent = fadeTime <= 0f
            ? 1f
            : Mathf.Clamp01(fadeTimer / fadeTime);

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(color.a, 0f, fadePercent);
            spriteRenderer.color = color;
        }

        if (fadePercent >= 1f)
            Destroy(gameObject);
    }
}