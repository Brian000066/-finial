using UnityEngine;

public class MovingGround : MonoBehaviour
{
    public float moveSpeed = 2f;

    private bool canMove = false;

    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
    }

    public void StartMoving()
    {
        canMove = true;
    }
}