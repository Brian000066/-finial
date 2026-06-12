using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    public FallingGround fallingGround;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            fallingGround.StartFalling();
        }
    }
}