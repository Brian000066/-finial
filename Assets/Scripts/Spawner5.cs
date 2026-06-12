using UnityEngine;

public class Spawner5 : MonoBehaviour
{
    [System.Serializable]
    public struct TrashData
    {
        public GameObject prefab;
        public float fallSpeed; 
    }

    [Header("繫結設定")]
    public Rigidbody2D playerRb; 

    [Header("垃圾設定")]
    public TrashData[] trashTypes;

    [Header("時空同步參數")]
    [Tooltip("垃圾從玩家頭頂上方多高的距離落下 (相對高度)")]
    public float heightAbovePlayer = 10f;

    [Tooltip("基礎安全距離（當速度極慢時的最短後方距離）")]
    public float baseSafeDistance = 1.5f;

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

        float playerX = playerRb.transform.position.x;
        float playerY = playerRb.transform.position.y;

        // 1. 計算世界 Y 軸生成座標與正數的掉落時間
        float actualSpawnY = playerY + heightAbovePlayer; 
        float fallTime = heightAbovePlayer / selectedTrash.fallSpeed;

        // 2. 取得玩家當前 X 軸速度
        float currentSpeed = playerRb.linearVelocity.x;
        float spawnX = 0f;

        // 3. 核心動態判斷
        if (currentSpeed > 5f) 
        {
            // 【物理數學修正】：為了防止烏龜在半空撞上垃圾，
            // 安全距離必須隨著「烏龜速度 / 垃圾掉落速度」的比例動態放大！
            // 這裡的 2.5f 代表烏龜與垃圾的 Collider 高度加總預估值，可依手感微調
            float dynamicSafeDistance = baseSafeDistance + (2.5f * (currentSpeed / selectedTrash.fallSpeed));
            
            spawnX = playerX + (currentSpeed * fallTime) - dynamicSafeDistance;
        }
        else 
        {
            // 如果玩家停下來或往回走：直接生在頭頂
            spawnX = playerX + Random.Range(-0.5f, 0.5f);
        }

        // 生成垃圾
        Vector3 spawnPosition = new Vector3(spawnX, actualSpawnY, 0f);
        GameObject trash = Instantiate(selectedTrash.prefab, spawnPosition, Quaternion.identity);

        // 傳遞組件資訊
        ConstantFall moveScript = trash.GetComponent<ConstantFall>();
        if (moveScript == null) moveScript = trash.AddComponent<ConstantFall>();
        
        moveScript.playerRb = this.playerRb; 
        moveScript.speed = selectedTrash.fallSpeed;
    }
}

// 讓垃圾單純往下移動的小腳本
public class ConstantFall5 : MonoBehaviour
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
        float destroyHeight = playerRb.transform.position.y - 10f; 

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