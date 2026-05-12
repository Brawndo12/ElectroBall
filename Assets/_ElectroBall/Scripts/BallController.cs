using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Ball Physics")]
    [SerializeField] private float maxSpeed = 14f;
    [SerializeField] private float playerHitBoost = 4f;

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

        Vector2 direction = (rb.position - collision.rigidbody.position).normalized;
        rb.AddForce(direction * playerHitBoost, ForceMode2D.Impulse);
    }
}