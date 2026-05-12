using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Ball Physics")]
    [SerializeField] private float maxSpeed = 18f;

    [Header("Player Hit Response")]
    [SerializeField] private float baseHitBoost = 2f;
    [SerializeField] private float speedHitMultiplier = 0.7f;
    [SerializeField] private float maxHitBoost = 10f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        if (collision.rigidbody == null) return;

        Vector2 hitDirection = (rb.position - collision.rigidbody.position).normalized;

        float playerSpeed = collision.rigidbody.velocity.magnitude;
        float hitBoost = baseHitBoost + playerSpeed * speedHitMultiplier;
        hitBoost = Mathf.Min(hitBoost, maxHitBoost);

        rb.AddForce(hitDirection * hitBoost, ForceMode2D.Impulse);
    }
}