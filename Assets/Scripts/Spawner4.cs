using UnityEngine;

public class Spawner4 : MonoBehaviour
{
    [System.Serializable]
    public struct TrashData
    {
        public GameObject prefab;
        public float fallSpeed; 
    }

    [Header("繫結設定")]
    public Rigidbody2D playerRb; // 繫結烏龜的 Rigidbody2D

    [Header("垃圾設定")]
    public TrashData[] trashTypes;

    [Header("時空同步參數")]
    [Tooltip("垃圾從玩家頭頂上方多高的距離落下 (相對高度)")]
    public float heightAbovePlayer = 10f;

    [Tooltip("剛好避開時，垃圾落在角色後方多遠的距離 (D)。請使用正數。")]
    public float safeDistance = 1.5f;

    [Tooltip("生成間隔秒數")]
    public float spawnInterval = 0.3f; 

    private float timer;

    void Update()
    {
        if (playerRb == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTrash();
            timer = 0f;
        }
    }

    void SpawnTrash()
    {
        int randomIndex = Random.Range(0, trashTypes.Length);
        TrashData selectedTrash = trashTypes[randomIndex];

        // 取得玩家位置
        float playerX = playerRb.transform.position.x;
        float playerY = playerRb.transform.position.y;

        // 【修正 1】：真正的 Y 軸生成「座標」(實際位置)
        float actualSpawnY = playerY + heightAbovePlayer; 

        // 【修正 2】：計算掉落時間。必須用「相對高度(距離)」來除以速度，確保時間永遠是正數！
        float fallTime = heightAbovePlayer / selectedTrash.fallSpeed;

        // 取得玩家當前真實速度 (X軸)
        float currentSpeed = playerRb.linearVelocity.x;
        float spawnX = 0f;

        // 核心動態判斷
        if (currentSpeed > 5f) 
        {
            // 如果玩家在前進：算好提早量，讓垃圾剛好落在身後
            spawnX = playerX + (currentSpeed * fallTime) - safeDistance;
        }
        else 
        {
            // 如果玩家停下來或往回走：直接生在頭頂
            spawnX = playerX + Random.Range(-0.5f, 0.5f);
        }

        // 確保 Speed 和 SpawnX 在玩家移動時會變動
        Debug.Log($"[Spawner] Speed: {currentSpeed:F1}, FallTime: {fallTime:F2}, SpawnX: {spawnX:F1}");

        // 生成垃圾 (使用 actualSpawnY 作為高度)
        Vector3 spawnPosition = new Vector3(spawnX, actualSpawnY, 0f);
        GameObject trash = Instantiate(selectedTrash.prefab, spawnPosition, Quaternion.identity);

        // 獲取或加入 ConstantFall 腳本
        ConstantFall moveScript = trash.GetComponent<ConstantFall>();
        if (moveScript == null) 
        {
            moveScript = trash.AddComponent<ConstantFall>();
        }

        // 把 Spawner 認識的烏龜 (playerRb)，交給剛剛生成的垃圾
        moveScript.playerRb = this.playerRb; 
        moveScript.speed = selectedTrash.fallSpeed;
    }
}

// 讓垃圾單純往下移動的小腳本
public class ConstantFall : MonoBehaviour
{
    [Header("繫結設定")]
    public Rigidbody2D playerRb; 

    public float speed;

    void Update()
    {
        // 安全鎖：如果沒綁定到烏龜，就直接往下掉並靠時間銷毀，防止報錯卡死
        if (playerRb == null)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            return;
        }

        // 根據玩家當前高度，計算動態銷毀線
        float destroyHeight = playerRb.transform.position.y - 30f; 

        // 往下移動
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        // 超過玩家下方一定距離就摧毀
        if (transform.position.y < destroyHeight) 
        {
            Destroy(gameObject);
        }
    }

    // 建議養成習慣加上 Start 萬用保險，防止垃圾沒掉到位在後台無限累積
    void Start()
    {
        Destroy(gameObject, 5f); 
    }
}
