using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController6 : MonoBehaviour
{
    [Header("玩家狀態 (Player Stats)")]
    public float maxLives = 100f;
    private float currentLives;
    public float moveSpeed = 30f;
    public float jumpForce = 75f;
    public float drainRate = 10f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDead = false;
    private bool isCleared = false;
    private Vector3 startPosition;
    private Vector3 lastGroundedPos;
    private Vector3 originalScale;

    [Header("二段跳設定")]
    public int maxJumps = 2;
    private int jumpsRemaining;

    [Header("掉落重置")]
    public float fallThreshold = -40f;

    [Header("環境檢查")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("UI 元素")]
    public Image hpBarFill;
    public TextMeshProUGUI centerMessageText;

    [Header("場景設定")]
    public string worldMapSceneName = "WorldMap";
    public float returnDelay = 2f;

    public void ShowMessage(string message)
    {
        if (centerMessageText != null)
        {
            centerMessageText.text = message;
        }

        Invoke("ClearMessage", 2f);
    }

    void ClearMessage()
    {
        if (centerMessageText != null && !isDead && !isCleared)
        {
            centerMessageText.text = "";
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        currentLives = maxLives;
        jumpsRemaining = maxJumps;
        lastGroundedPos = transform.position;
        startPosition = transform.position;

        if (centerMessageText != null)
        {
            centerMessageText.text = "";
        }

        UpdateHealthUI();

        ShowMessage("Run!!!!!", Color.red);
        StartCoroutine(ClearMessageAfterDelay(3f));
    }

    void OnMove(InputValue value)
    {
        if (!isDead && !isCleared)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && jumpsRemaining > 0 && !isDead && !isCleared)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining--;
        }
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

        if (isCleared)
        {
            return;
        }

        // P = 快速通關
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            LevelClear();
            return;
        }

        isGrounded = Physics2D.OverlapBox(
            groundCheck.position,
            groundCheckSize,
            0f,
            groundLayer
        );

        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            lastGroundedPos = transform.position;
            jumpsRemaining = maxJumps;
        }

        if (transform.position.y < fallThreshold)
        {
            FallReset();
        }

        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }

        currentLives -= drainRate * Time.deltaTime;
        currentLives = Mathf.Max(currentLives, 0f);

        UpdateHealthUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isDead || isCleared) return;

        float horizontalSpeed = 0;

        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            horizontalSpeed = Mathf.Sign(moveInput.x) * moveSpeed;
        }

        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
    }

    void UpdateHealthUI()
    {
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = currentLives / maxLives;
        }
    }

    void FallReset()
    {
        TakeDamage(100f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isCleared) return;

        if (other.CompareTag("Trash"))
        {
            other.enabled = false;
            other.gameObject.SetActive(false);
            TakeDamage(10f);
        }
        else if (other.CompareTag("HealingRoom"))
        {
            other.enabled = false;
            other.gameObject.SetActive(false);
            Heal(30f);
        }
        else if (other.CompareTag("Door"))
        {
            LevelClear();
        }
    }

    void TakeDamage(float damage)
    {
        if (isDead || isCleared) return;

        currentLives -= damage;
        UpdateHealthUI();

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Heal(float amount)
    {
        currentLives += amount;

        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        UpdateHealthUI();
    }

    void LevelClear()
    {
        if (isCleared) return;

        isCleared = true;
        GameState.egyptCleared = true;
        rb.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        centerMessageText.fontSize = 40;
        ShowMessage("You Escape!!!", Color.green);

        StartCoroutine(ReturnToWorldMapAfterDelay());
    }

    IEnumerator ReturnToWorldMapAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(worldMapSceneName);
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        centerMessageText.fontSize = 30;
        ShowMessage(
            "You have been contaminated.\nPress 'R' to Restart\nPress 'M' to Map",
            Color.red
        );
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

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}