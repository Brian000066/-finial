using UnityEngine;

public class TurtleWalkAnimator : MonoBehaviour
{
    public Sprite[] walkSprites;
    public float frameTime = 0.15f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;

    private float originalScaleX;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 記住你在 Inspector 設定的原本大小
        originalScaleX = Mathf.Abs(transform.localScale.x);
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        // 有移動才播放動畫
        if (moveX != 0 && walkSprites.Length > 0)
        {
            timer += Time.deltaTime;

            if (timer >= frameTime)
            {
                timer = 0f;
                currentFrame++;

                if (currentFrame >= walkSprites.Length)
                {
                    currentFrame = 0;
                }

                spriteRenderer.sprite = walkSprites[currentFrame];
            }
        }
        else
        {
            currentFrame = 0;

            if (walkSprites.Length > 0)
            {
                spriteRenderer.sprite = walkSprites[0];
            }
        }

        // 只翻左右，不改變大小
        if (moveX > 0)
        {
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
}