using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnockback = false;

    public float spikeDamage = 20f;
    public float knockbackForceX = 6f;
    public float knockbackForceY = 4f;
    public float knockbackTime = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike") && !isKnockback)
        {
            PlayerHealth health = GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(spikeDamage);
            }

            StartCoroutine(Knockback(other.transform));
        }
    }

    IEnumerator Knockback(Transform spike)
    {
        isKnockback = true;

        float direction = transform.position.x > spike.position.x ? 1f : -1f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(
            new Vector2(direction * knockbackForceX, knockbackForceY),
            ForceMode2D.Impulse
        );

        yield return new WaitForSeconds(knockbackTime);

        isKnockback = false;
    }
}