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

<<<<<<< HEAD
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ForceWin();
        }
    }

=======
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
    void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
<<<<<<< HEAD
            ForceWin();

=======
            finished = true;

            // 自動抓目前 Scene 名稱
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


            // 停止生命流逝
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.StopHealthDrain();
            }

<<<<<<< HEAD
=======
            // 停止物理移動
>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
<<<<<<< HEAD
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

=======

            StartCoroutine(WinRoutine());
        }
    }

>>>>>>> b0b843043e5ca1bc77246e8da03535189231dce3
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