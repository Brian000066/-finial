using UnityEngine;

public enum BlockType { Normal, Black, NeonGreen }

public class Block : MonoBehaviour
{
    public BlockType currentType = BlockType.Normal;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        SetType(BlockType.Normal);
    }

    public void SetType(BlockType newType)
    {
        currentType = newType;
        switch (currentType)
        {
            case BlockType.Normal:
                rend.material.color = Color.gray; // 預設灰色
                break;
            case BlockType.Black:
                rend.material.color = Color.red; // 黑色污染
                break;
            case BlockType.NeonGreen:
                rend.material.color = new Color(0.2f, 1f, 0.2f); // 螢光綠
                break;
        }
    }
}
