using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] trashPrefabs; 
    public GameObject healPrefab;     
    public GameObject keyPrefab;      

    [Header("Key Sprites")]
    public Sprite[] keySprites;       

    [Header("Spawn Settings")]
    public float spawnRate = 2f;      
    public float spawnX = 60f;        

    [Header("Difficulty Settings")]
    public float baseSpeed = 20f;     // 初始移動速度
    private float currentSpeed;       // 當前的移動速度

    private float timer = 0f;
    private int spawnCount = 0;

    private float minY = -28f;
    private float maxY = 10f; 

    void Start()
    {
        // 遊戲開始時，速度設為初始速度
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnWave();
            timer = 0f;
        }
    }

    void SpawnWave()
    {
        spawnCount++;

        // 【新功能】每 15 波，所有生成物的速度 +5
        if (spawnCount % 15 == 0)
        {
            currentSpeed += 10f;

            if (spawnRate > 0.6f) 
            {
                spawnRate -= 0.3f;
            }

        }

        // 1. 每 10 次生成一個鑰匙碎片（最多 5 個）
        if (spawnCount % 10 == 0 && PlayerController2.keyFragments < 5)
        {
            if (keyPrefab != null)
            {
                GameObject newKey = SpawnObject(keyPrefab);
                
                // 安全檢查：確認有 SpriteRenderer 才替換圖片
                SpriteRenderer sr = newKey.GetComponent<SpriteRenderer>();

                // 關鍵：直接用玩家「當前得到的數量」作為下一把鑰匙的圖片索引
                // 假設目前吃到了 2 把 (keysCollected = 2)，那代表第 0, 1 把拿到了，這波就生成第 2 把（第三張圖）
                // 如果這把被玩家漏掉了，keysCollected 依然是 2，下個 10 波就會再次生成第 2 把！
                int currentKeyIndex = PlayerController2.keyFragments;
                
                if (sr != null && keySprites.Length > currentKeyIndex)
                {
                    sr.sprite = keySprites[currentKeyIndex];
                }
            }
        }

        // 2. 每 7 次生成一個回血道具
        if (spawnCount % 7 == 0)
        {
            SpawnObject(healPrefab);
        }

        // 3. 生成垃圾
        int trashAmount = Random.Range(1, 4); 
        for (int i = 0; i < trashAmount; i++)
        {
            GameObject randomTrash = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
            SpawnObject(randomTrash);
        }
    }

    // 負責處理實例化，並把當前的速度設定給物件
    GameObject SpawnObject(GameObject prefab)
    {
        float randomY = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(spawnX, randomY);
        GameObject newObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        // 將當前的速度套用到該物件的 MoveLeft 腳本上
        MoveLeft2 moveScript = newObj.GetComponent<MoveLeft2>();
        if (moveScript != null)
        {
            moveScript.speed = currentSpeed;
        }

        return newObj;
    }
}
