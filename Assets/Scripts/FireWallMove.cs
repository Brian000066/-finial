using UnityEngine;

public class FireWallMove : MonoBehaviour
{
    public float speed = 2f;
    public float acceleration = 0.1f;

    void Update()
    {
        speed += acceleration * Time.deltaTime;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}