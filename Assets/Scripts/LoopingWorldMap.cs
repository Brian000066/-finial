using UnityEngine;

public class LoopingWorldMap : MonoBehaviour
{
    public Transform mapA;
    public Transform mapB;

    public float moveSpeed = 5f;
    public float mapWidth = 18f;

    public float minY = -4f;
    public float maxY = 4f;

    private float currentY = 0f;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f).normalized;

        mapA.position -= new Vector3(move.x, 0, 0) * moveSpeed * Time.deltaTime;
        mapB.position -= new Vector3(move.x, 0, 0) * moveSpeed * Time.deltaTime;

        currentY -= move.y * moveSpeed * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        mapA.position = new Vector3(mapA.position.x, currentY, mapA.position.z);
        mapB.position = new Vector3(mapB.position.x, currentY, mapB.position.z);

        if (mapA.position.x <= -mapWidth)
        {
            mapA.position = new Vector3(mapB.position.x + mapWidth, currentY, mapA.position.z);
        }

        if (mapB.position.x <= -mapWidth)
        {
            mapB.position = new Vector3(mapA.position.x + mapWidth, currentY, mapB.position.z);
        }

        if (mapA.position.x >= mapWidth)
        {
            mapA.position = new Vector3(mapB.position.x - mapWidth, currentY, mapA.position.z);
        }

        if (mapB.position.x >= mapWidth)
        {
            mapB.position = new Vector3(mapA.position.x - mapWidth, currentY, mapB.position.z);
        }
    }
}