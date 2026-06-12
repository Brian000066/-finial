using UnityEngine;

public class FallingGround : MonoBehaviour
{
    public float fallSpeed = 5f;

    private bool isFalling = false;

    void Update()
    {
        if (isFalling)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    public void StartFalling()
    {
        isFalling = true;
    }
}