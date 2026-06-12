using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    public Transform target;       // 拖入你的海龜主角
    public float smoothSpeed = 0.125f; // 鏡頭跟隨的平滑速度
    public Vector3 offset = new Vector3(0, 0, -10); // 鏡頭與主角的相對距離

    void LateUpdate() // 使用 LateUpdate 確保在主角移動完後才移動鏡頭
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            // 讓鏡頭平滑移動到目標位置
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
