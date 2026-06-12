using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private float lastFallTime;
    public float fallSpeed = 0.8f;

    void Update()
    {
        if (GameManager.instance.isGameOver) return;

        // 左右移動
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(new Vector3(-1, 0, 0));
        if (Input.GetKeyDown(KeyCode.RightArrow)) Move(new Vector3(1, 0, 0));

        
        if (Input.GetKeyDown(KeyCode.UpArrow)) RotatePiece(new Vector3(0, 0, -90));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        // 往下掉落
        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFallTime >= fallSpeed)
        {
            Fall();
        }
    }

    void Fall()
    {
        transform.position += new Vector3(0, -1, 0);
        if (!IsValidGridPos())
        {
            transform.position += new Vector3(0, 1, 0);
            LockAndSpawnNext();
        }
        lastFallTime = Time.time;
    }

    void HardDrop()
    {
        // 不斷往下移動，直到位置無效
        while (IsValidGridPos())
        {
            transform.position += new Vector3(0, -1, 0);
        }

        // 撞到邊界或方塊了，往回退一格
        transform.position += new Vector3(0, 1, 0);

        // 立刻鎖定並生成下一個
        LockAndSpawnNext();
    }

    void Move(Vector3 direction)
    {
        transform.position += direction;
        if (!IsValidGridPos()) transform.position -= direction;
    }

    void RotatePiece(Vector3 rotation)
    {
        transform.Rotate(rotation);
        if (!IsValidGridPos()) transform.Rotate(-rotation); // 碰撞就轉回來
    }

    public bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = new Vector2(Mathf.Round(child.position.x), Mathf.Round(child.position.y));
            if (v.x < 0 || v.x >= GameManager.width || v.y < 0) return false;
            if (v.y < GameManager.height && GameManager.instance.grid[(int)v.x, (int)v.y] != null) return false;
        }
        return true;
    }

    void LockAndSpawnNext()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = new Vector2(Mathf.Round(child.position.x), Mathf.Round(child.position.y));
            
            // 判死條件 2：任何格子鎖定在頂端 (Y=20) 或更高處，直接判輸
            if (v.y >= GameManager.height)
            {
                GameManager.instance.GameOver();
                return;
            }
            GameManager.instance.grid[(int)v.x, (int)v.y] = child;
        }

        GameManager.instance.CheckLines();
        enabled = false;
        FindObjectOfType<Spawner7>().SpawnPiece();
    }
}
