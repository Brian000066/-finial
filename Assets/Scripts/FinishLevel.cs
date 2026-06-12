using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishLevel : MonoBehaviour
{
    public GameObject winUI;

    private bool finished = false;

    void Start()
    {
        if (winUI != null)
        {
            winUI.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ForceWin();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            ForceWin();

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.StopHealthDrain();
            }

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    void ForceWin()
    {
        if (finished) return;

        finished = true;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level_China")
        {
            GameState.chinaCleared = true;
        }

        if (currentScene == "Level_alabo")
        {
            GameState.alaboCleared = true;
        }

        if (currentScene == "Level_India")
        {
            GameState.indiaCleared = true;
        }

        if (currentScene == "Level_nilsilan")
        {
            GameState.nilsilanCleared = true;
        }

        if (currentScene == "Level_Australia")
        {
            GameState.australiaCleared = true;
        }

        if (currentScene == "Level_SouthAfrica")
        {
            GameState.southAfricaCleared = true;
        }

        if (currentScene == "Level_Antarctica")
        {
            GameState.antarcticaCleared = true;
        }

        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("WorldMap");
    }
}