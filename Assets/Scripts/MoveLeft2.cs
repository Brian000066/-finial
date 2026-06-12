using UnityEngine;

public class MoveLeft2 : MonoBehaviour
{
    public float speed = 20f;
    private float despawnX = -60f; // 離開畫面左側的銷毀座標

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}
