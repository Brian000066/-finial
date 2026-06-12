using UnityEngine;

public class TrashDropTrigger : MonoBehaviour
{
    public GameObject trashPrefab;
    public Transform dropPoint;
    public bool onlyOnce = true;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onlyOnce && triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;
            Instantiate(trashPrefab, dropPoint.position, Quaternion.identity);
        }
    }
}