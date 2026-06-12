using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPoint : MonoBehaviour
{
    public string sceneName;

    public Transform map0;
    public Transform map1;

    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            WorldMapState.map0Position = map0.position;
            WorldMapState.map1Position = map1.position;
            WorldMapState.hasSavedMapPosition = true;

            SceneManager.LoadScene(sceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Press E to enter level");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}