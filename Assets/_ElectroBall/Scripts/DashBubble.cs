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

    private float timer;

    public void Initialize(
        float diameter,
        float growTime,
        float existTime,
        float impulse
    )
    {
        targetDiameter = diameter;
        growthTime = growTime;
        lifetime = existTime;
        ballImpulse = impulse;

        bubbleCollider = GetComponent<CircleCollider2D>();
        bubbleCollider.isTrigger = true;
        bubbleCollider.radius = 0f;

        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float growPercent = growthTime <= 0f
            ? 1f
            : Mathf.Clamp01(timer / growthTime);

        float currentDiameter = targetDiameter * growPercent;

        transform.localScale = Vector3.one * currentDiameter;
        bubbleCollider.radius = 0.5f;

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0.35f, 0f, timer / lifetime);
            spriteRenderer.color = color;
        }

        if (timer >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryHitBall(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryHitBall(other);
    }

    private void TryHitBall(Collider2D other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Rigidbody2D ballRb = other.attachedRigidbody;

        if (ballRb == null)
            return;

        Vector2 direction = (ballRb.position - (Vector2)transform.position).normalized;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.up;

        ballRb.AddForce(direction * ballImpulse, ForceMode2D.Impulse);
    }
}