using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DontMoveChallenge : MonoBehaviour
{
    public float surviveTime = 15f;
    public TextMeshProUGUI timerText;

    private float timer = 0f;
    private bool ended = false;

    void Update()
    {
        if (ended) return;

        bool moved =
            Input.GetAxisRaw("Horizontal") != 0 ||
            Input.GetAxisRaw("Vertical") != 0 ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow);

        if (moved)
        {
            ended = true;

            GameOverManager gm = FindFirstObjectByType<GameOverManager>();
            if (gm != null)
            {
                gm.GameOver();
            }

            return;
        }

        timer += Time.deltaTime;

        float remain = surviveTime - timer;

        if (timerText != null)
        {
            timerText.text = "Don't Move : " + Mathf.CeilToInt(remain);
        }

        if (timer >= surviveTime)
        {
            ended = true;
            SceneManager.LoadScene("WorldMap");
        }
    }
}