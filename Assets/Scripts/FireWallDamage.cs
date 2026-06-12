using UnityEngine;
using UnityEngine.SceneManagement;

public class FireWallDamage : MonoBehaviour
{
    public GameObject gameOverText;
    public string worldMapScene = "WorldMap";

    private bool dead = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
    }

    void Update()
    {
        if (!dead) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(worldMapScene);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dead) return;

        if (other.CompareTag("Player"))
        {
            dead = true;

            if (gameOverText != null)
            {
                gameOverText.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}