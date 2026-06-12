using UnityEngine;
using TMPro; // 建議使用 TextMeshPro 來處理文字 UI
using UnityEngine.UI; // 必須引用這個來控制 Image
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // 必須引入這個才能控制關卡重新開始

public class PlayerController5 : MonoBehaviour
{
    [Header("玩家狀態 (Player Stats)")]
    public float maxLives = 5;
    private float currentLives;
    public float moveSpeed = 50f;
    public float jumpForce = 75f; // 跳躍力度
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isGrounded;
        private bool isDead = false;
        private Vector3 startPosition; // 記錄初始位置
    private Vector3 originalScale;

    [Header("二段跳設定")]
    public int maxJumps = 2; // 最大跳躍次數 (設定 2 就是二段跳)
    private int jumpsRemaining; // 目前剩餘跳躍次數

    [Header("掉落重置")]
    public float fallThreshold = -40f; // 低於這個高度就判定掉落
    private Vector3 lastGroundedPos;   // 記錄最後在地面上的位置
    
    [Header("環境檢查")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.8f, 0.1f); // 寬度 0.8，高度 0.1 的扁平矩形(線)
    public float checkRadius = 0.2f;
    public LayerMask groundLayer; // 設定地面的圖層
    

    [Header("UI 元素")]
    public Image[] heartImages;    // 拖入 5 個愛心的 UI Image
    public TextMeshProUGUI centerMessageText; // 畫面中間的文字 UI

    [Header("場景設定")]
    public string worldMapSceneName = "WorldMap";
    public float returnDelay = 2f;

    private bool isCleared = false;

    public void ShowMessage(string message)
    {
        centerMessageText.text = message;
        // 這裡可以加一個邏輯讓字在 2 秒後消失
        Invoke("ClearMessage", 2f);
    }

    void ClearMessage()
    {
        centerMessageText.text = "";
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 遊戲開始時初始化生命值並清空訊息
        currentLives = maxLives;
        jumpsRemaining = maxJumps;
        lastGroundedPos = transform.position;
        startPosition = transform.position;
        originalScale = transform.localScale;

        if (centerMessageText != null)
        {
            centerMessageText.text = ""; 
        }
        
        
        ShowMessage("Be Brave!!",Color.red); // 遊戲開始提示
        StartCoroutine(ClearMessageAfterDelay(3f)); // 3秒後消失
    }

    void OnMove(InputValue value) { if (!isDead) moveInput = value.Get<Vector2>(); }

    void OnJump(InputValue value)
    {
        // 條件改為：按下按鍵 且「還有剩餘跳躍次數」
        if (value.isPressed && jumpsRemaining > 0 && !isDead)
        {
            // 每次跳躍前先歸零 Y 軸速度，確保空中的第二跳高度和第一跳一樣高
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            jumpsRemaining--; // 消耗一次跳躍額度
        }

    }

    void Update()
    {
        if (isDead)
        {
            // 死亡後，偵測是否按下 R 鍵
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
            CheckDoor();
            return;
        }
        // 偵測是否落地
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        
        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            lastGroundedPos = transform.position; // 更新最後的落地位置
            jumpsRemaining = maxJumps; // 回復跳躍次數
        }

        // 2. 掉落偵測 (高度判斷)
        if (transform.position.y < fallThreshold)
        {
            FallReset();
        }


        // 3. 轉向邏輯
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


        // 3. 檢查是否死亡
        if (currentLives <= 0)
        {
            Die();
        }

    }

    void FixedUpdate()
    {
        if (isDead) return;

        // 為了確保速度不會被輸入系統的搖桿影響而太慢，我們用 Mathf.Sign 強制讓移動為全速
        // 只要有按方向，速度就是滿的
        float horizontalSpeed = 0;
        if (Mathf.Abs(moveInput.x) > 0.1f) 
        {
            horizontalSpeed = Mathf.Sign(moveInput.x) * moveSpeed;
        }
        
        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
    
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentLives);
        }
    }

    void FallReset()
    {
        TakeDamage(5); // 掉落扣一滴血
        if (!isDead)
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = lastGroundedPos; // 返回最後安全地面
        }
    }


    // 處理觸發器碰撞 (適用於可以穿透的物件，如收集品、區域)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        // 1. 碰到垃圾 (扣 1 條命)
        if (other.CompareTag("Trash"))
        {
            other.enabled = false; // 第一時間關閉對方的碰撞體！杜絕二次觸發
            other.gameObject.SetActive(false); 
            TakeDamage(1);
        }
        // 2. 進入房間 (回 3 條命)
        else if (other.CompareTag("HealingRoom"))
        {
            other.enabled = false; // 一樣，第一時間關閉碰撞體
            other.gameObject.SetActive(false); 
            Heal(3);
        }
        // 4. 碰到終點門
        else if (other.CompareTag("Door"))
        {
            CheckDoor();
        }
    }

    // 扣血邏輯
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

    // 回血邏輯
    void Heal(int amount)
    {
        currentLives += amount;
        UpdateHeartUI();
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }
    }

    // 檢查終點門邏輯
    void CheckDoor()
    {
        if (isCleared) return;

        isCleared = true;
        GameState.italyCleared = true;

        centerMessageText.fontSize = 40;
        ShowMessage("You Escape!!!", Color.green);

        rb.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;
        currentLives = 100;

        StartCoroutine(ReturnToWorldMapAfterDelay());
    }

    // 死亡邏輯
    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        centerMessageText.fontSize = 30;
        ShowMessage(
    "You have been contaminated.\nPress 'R' to Restart\nPress 'M' to Map",Color.red);
    }

    // 顯示訊息的共用方法
    void ShowMessage(string msg, Color color)
    {
        if (centerMessageText != null)
        {
            centerMessageText.text = msg;
            centerMessageText.color = color;
        }
    }

    // 延遲清除訊息的協程 (用於 "Need a key" 這種暫時提示)
    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isDead && centerMessageText.text != "You stopped the factory.") 
            centerMessageText.text = "";
    }

    IEnumerator ReturnToWorldMapAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(worldMapSceneName);
    }

    void ResetGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green; // 用綠色線框顯示
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }

}
