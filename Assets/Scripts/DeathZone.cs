using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameOverManager gm = FindFirstObjectByType<GameOverManager>();

            if (gm != null)
            {
                gm.GameOver();
            }
        }
    }
}