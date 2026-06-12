using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController1 : MonoBehaviour
{
    [Header("玩家狀態 (Player Stats)")]
    public int maxLives = 5;
    private int currentLives;
    private int keyFragments = 0;
    private const int totalKeysNeeded = 5;
    public float moveSpeed = 30f;
    public float jumpForce = 75f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDead = false;
    private Vector3 startPosition;
    private Vector3 originalScale;

    [Header("二段跳設定")]
    public int maxJumps = 2;
    private int jumpsRemaining;

    [Header("掉落重置")]
    public float fallThreshold = -40f;
    private Vector3 lastGroundedPos;

    [Header("UI 元件 (UI References)")]
    public TextMeshProUGUI centerMessageText;
    public Image[] heartImages;
    public TextMeshProUGUI keyCountText;

    [Header("環境檢查")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("場景設定")]
    public string worldMapSceneName = "WorldMap";
    public float clearDelay = 2f;

    void Start()
    {
        Time.timeScale = 1f;

        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody2D>();

        currentLives = maxLives;
        lastGroundedPos = transform.position;
        jumpsRemaining = maxJumps;
        startPosition = transform.position;

        if (centerMessageText != null)
        {
            centerMessageText.text = "";
        }

        UpdateHeartUI();
        UpdateKeyUI();

        ShowMessage("Let's stop the factory!", Color.white);
        StartCoroutine(ClearMessageAfterDelay(3f));
    }

    void OnMove(InputValue value)
    {
        if (!isDead)
            moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && jumpsRemaining > 0 && !isDead)
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

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
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

        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(
            Mathf.Sign(moveInput.x) * Mathf.Abs(originalScale.x),
                    originalScale.y,
                    originalScale.z
            );
        }
        // P = 快速通關
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            keyFragments = 5;
            CheckDoor();
            return;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        float horizontalSpeed = 0;

        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            horizontalSpeed = Mathf.Sign(moveInput.x) * moveSpeed;
        }

        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
    }

    void FallReset()
    {
        TakeDamage(1);

        if (!isDead)
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = lastGroundedPos;
        }
    }

    void UpdateKeyUI()
    {
        if (keyCountText != null)
        {
            keyCountText.text = "Keys: " + keyFragments + " / " + totalKeysNeeded;
        }

        if (keyFragments >= totalKeysNeeded)
        {
            centerMessageText.fontSize = 40;
            ShowMessage("Go find the door!", Color.yellow);
            StopAllCoroutines();
            StartCoroutine(ClearMessageAfterDelay(2f));
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
        if (isDead) return;

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
        else if (other.CompareTag("Door"))
        {
            CheckDoor();
        }
    }

    void TakeDamage(int damage)
    {
        if (isDead) return;

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

    void CheckDoor()
    {
        if (keyFragments >= totalKeysNeeded)
        {
            centerMessageText.fontSize = 40;
            ShowMessage("You stopped the factory.", Color.green);

            GameState.southAfricaCleared = true;

            rb.linearVelocity = Vector2.zero;
            moveInput = Vector2.zero;
            isDead = true;

            StartCoroutine(ReturnToMapAfterDelay());
        }
        else
        {
            centerMessageText.fontSize = 30;
            ShowMessage("Need a key", Color.red);
            StopAllCoroutines();
            StartCoroutine(ClearMessageAfterDelay(2f));
        }
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

        if (!isDead && centerMessageText != null)
        {
            centerMessageText.text = "";
        }
    }

    IEnumerator ReturnToMapAfterDelay()
    {
        yield return new WaitForSeconds(clearDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(worldMapSceneName);
    }

    void ResetGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}