using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    [Header("玩家狀態 (Player Stats)")]
    public int maxLives = 5;
    private int currentLives;
    public static int keyFragments = 0;
    private const int totalKeysNeeded = 5;
    public float jumpForce = 75f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDead = false;
    private bool isCleared = false;

    [Header("UI 元件 (UI References)")]
    public TextMeshProUGUI centerMessageText;
    public Image[] heartImages;
    public TextMeshProUGUI keyCountText;

    [Header("場景設定")]
    public string worldMapSceneName = "WorldMap";
    public float returnDelay = 2f;

    public void ShowMessage(string message)
    {
        centerMessageText.text = message;
        Invoke("ClearMessage", 2f);
    }

    void ClearMessage()
    {
        centerMessageText.text = "";
    }

    void Start()
    {
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        currentLives = maxLives;
        keyFragments = 0;

        if (centerMessageText != null)
        {
            centerMessageText.text = "";
        }

        UpdateHeartUI();
        UpdateKeyUI();

        ShowMessage("Let's stop the factory!", Color.white);
        StartCoroutine(ClearMessageAfterDelay(3f));
    }

    void Update()
    {
        if (isDead)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                ResetGame();
            }

            if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(worldMapSceneName);
            }

            return;
        }
        // P = 快速通關
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            LevelClear();
            return;
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && !isDead && !isCleared)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void UpdateKeyUI()
    {
        if (keyCountText != null)
            keyCountText.text = "Keys: " + keyFragments + " / " + totalKeysNeeded;

        if (keyFragments >= totalKeysNeeded && !isCleared)
        {
            LevelClear();
        }
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentLives);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isCleared) return;

        if (other.CompareTag("Trash"))
        {
            TakeDamage(1);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("HealingRoom"))
        {
            Heal(3);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("KeyFragment"))
        {
            keyFragments++;
            other.gameObject.SetActive(false);
            UpdateKeyUI();
        }
    }

    void TakeDamage(int damage)
    {
        if (isDead || isCleared) return;

        currentLives -= damage;
        UpdateHeartUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Heal(int amount)
    {
        currentLives += amount;

        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        UpdateHeartUI();
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        centerMessageText.fontSize = 30;
        ShowMessage(
            "You have been contaminated.\nPress 'R' to Restart\nPress 'M' to Map",
            Color.red
        );
    }

    void LevelClear()
    {
        isCleared = true;
        rb.linearVelocity = Vector2.zero;

        centerMessageText.fontSize = 40;
        ShowMessage("You stopped the factory.", Color.green);

        GameState.antarcticaCleared = true;
        
        StartCoroutine(ReturnToWorldMapAfterDelay());
    }

    IEnumerator ReturnToWorldMapAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(worldMapSceneName);
    }

    void ShowMessage(string msg, Color color)
    {
        if (centerMessageText != null)
        {
            centerMessageText.text = msg;
            centerMessageText.color = color;
        }
    }

    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isDead && !isCleared && centerMessageText != null)
        {
            centerMessageText.text = "";
        }
    }

    void ResetGame()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}