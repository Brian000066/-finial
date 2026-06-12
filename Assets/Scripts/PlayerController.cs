using UnityEngine;
using TMPro; // 建議使用 TextMeshPro 來處理文字 UI
using UnityEngine.UI; // 必須引用這個來控制 Image
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // 必須引入這個才能控制關卡重新開始

public class PlayerController : MonoBehaviour
{
    [Header("玩家狀態 (Player Stats)")]
    public int maxLives = 5;
    private int currentLives;
    private int keyFragments = 0;
    private const int totalKeysNeeded = 5;
    public float moveSpeed = 30f;
    public float jumpForce = 75f; // 跳躍力度
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isGrounded;
        private bool isDead = false;
        private Vector3 startPosition; // 記錄初始位置

    [Header("二段跳設定")]
    public int maxJumps = 2; // 最大跳躍次數 (設定 2 就是二段跳)
    private int jumpsRemaining; // 目前剩餘跳躍次數

    [Header("掉落重置")]
    public float fallThreshold = -40f; // 低於這個高度就判定掉落
    private Vector3 lastGroundedPos;   // 記錄最後在地面上的位置

    [Header("UI 元件 (UI References)")]
    public TextMeshProUGUI centerMessageText; // 畫面中間的文字 UI
    public Image[] heartImages;    // 拖入 5 個愛心的 UI Image
    public TextMeshProUGUI keyCountText;
    
    [Header("環境檢查")]
    public Transform groundCheck; // 在海龜腳下放一個空的 GameObject
    public float checkRadius = 0.2f;
    public LayerMask groundLayer; // 設定地面的圖層
    
    

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
        lastGroundedPos = transform.position;
        jumpsRemaining = maxJumps;
        startPosition = transform.position;

        if (centerMessageText != null)
        {
            centerMessageText.text = ""; 
        }
        
        UpdateHeartUI();
        UpdateKeyUI();
        
        ShowMessage("Let's stop the factory!",Color.white); // 遊戲開始提示
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
                ResetGame();
            }
            return; // 死亡狀態下不執行下方的扣血邏輯
        }
        // 偵測是否落地
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            lastGroundedPos = transform.position; // 只要站在地上就更新安全座標
            jumpsRemaining = maxJumps; // 回復跳躍次數
        }

        // 2. 掉落偵測 (高度判斷)
        if (transform.position.y < fallThreshold)
        {
            FallReset();
        }

        // 3. 轉向邏輯
        if (moveInput.x != 0) transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);


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

    void FallReset()
    {
        TakeDamage(1); // 掉落扣一滴血
        if (!isDead)
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = lastGroundedPos; // 返回最後安全地面
        }
    }

    void UpdateKeyUI()
    {
        if (keyCountText != null)
            keyCountText.text = "Keys: " + keyFragments + " / " + totalKeysNeeded;

        if (keyFragments >= totalKeysNeeded)
        {
            // 集齊碎片：綠色文字
            centerMessageText.fontSize = 40;
            ShowMessage("Go find the door!", Color.yellow); 
            StopAllCoroutines(); // 避免多個計時器衝突
            StartCoroutine(ClearMessageAfterDelay(2f)); // 2秒後清除文字
        }
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentLives);
        }
    }

    // 處理觸發器碰撞 (適用於可以穿透的物件，如收集品、區域)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        // 1. 碰到垃圾 (扣 1 條命)
        if (other.CompareTag("Trash"))
        {
            TakeDamage(1);
            // 如果碰到垃圾後要讓垃圾消失，可以解除下面這行的註解
             other.gameObject.SetActive(false); 
        }
        // 2. 進入房間 (回 3 條命)
        else if (other.CompareTag("HealingRoom"))
        {
            Heal(3);
            // 避免重複無限回血，進入後可以將該房間的觸發器關閉或隱藏
            other.gameObject.SetActive(false); 
        }
        // 3. 收集鑰匙碎片
        else if (other.CompareTag("KeyFragment"))
        {
            keyFragments++;
            other.gameObject.SetActive(false); // 收集後隱藏碎片
            UpdateKeyUI();
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
        // 最高不會超過 5 條命
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }
        UpdateHeartUI();
    }

    // 檢查終點門邏輯
    void CheckDoor()
    {
        if (keyFragments >= totalKeysNeeded)
        {
            // 集齊碎片：綠色文字
            centerMessageText.fontSize = 40;
            ShowMessage("You stopped the factory.", Color.green);
            Time.timeScale = 0;
        }
        else
        {
            // 未集齊：顯示提示後消失
            centerMessageText.fontSize = 30;
            ShowMessage("Need a key", Color.red); 
            StopAllCoroutines(); // 避免多個計時器衝突
            StartCoroutine(ClearMessageAfterDelay(2f)); // 2秒後清除文字
        }
    }

    // 死亡邏輯
    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        centerMessageText.fontSize = 30;
        ShowMessage("You have been contaminated.\nPress 'R' to Restart", Color.red);
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

    void ResetGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        
    }

}


