using UnityEngine;

public class HealItem : MonoBehaviour
{
    public float healAmount = 30f;
    public bool destroyAfterUse = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.Heal(healAmount);
            }

            if (destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}