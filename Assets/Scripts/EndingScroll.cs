using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScroll : MonoBehaviour
{
    public float scrollSpeed = 1.5f;
    public float stopY = 8f;
    public string backSceneName = "WorldMap";

    void Update()
    {
        if (transform.position.y < stopY)
        {
            transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(backSceneName);
        }
    }
}