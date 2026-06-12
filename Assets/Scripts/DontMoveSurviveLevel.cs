using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DontMoveSurviveLevel : MonoBehaviour
{
    public float surviveTime = 25f;
    public float maxHealth = 100f;
    public float drainSpeed = 5f;

    public Image healthBarFill;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;

    private float timer = 0f;
    private float health;
    private bool realGameOver = false;
    private bool cleared = false;

    void Start()
    {
        Time.timeScale = 1f;
        health = maxHealth;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        if (cleared) return;

        // 真死亡後：R/M 才有效
        if (realGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("WorldMap");
            }

            return;
        }

        // 只要動就真的 Game Over
        bool moved =
            Input.GetAxisRaw("Horizontal") != 0 ||
            Input.GetAxisRaw("Vertical") != 0 ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.M);

        if (moved)
        {
            RealGameOver();
            return;
        }

        // 倒數繼續跑
        timer += Time.deltaTime;

        // 生命慢慢扣
        health -= drainSpeed * Time.deltaTime;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateUI();

        // 生命歸零：只顯示提示，但不算輸
        if (health <= 0)
        {
            if (gameOverUI != null)
                gameOverUI.SetActive(true);
        }

        // 撐到 25 秒過關
        if (timer >= surviveTime)
        {
            cleared = true;
            Time.timeScale = 1f;
            GameState.alaboCleared = true;
            SceneManager.LoadScene("WorldMap");
        }
    }

    void RealGameOver()
    {
        realGameOver = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;

        if (timerText != null)
            timerText.text = "Don't Move : " + Mathf.CeilToInt(surviveTime - timer);
    }
}