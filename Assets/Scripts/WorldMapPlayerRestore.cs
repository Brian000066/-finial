using UnityEngine;

public class WorldMapPlayerRestore : MonoBehaviour
{
    void Start()
    {
        if (PlayerWorldPosition.hasSavedPosition)
        {
            transform.position = PlayerWorldPosition.savedPosition;
        }
    }
}