using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float yOffset = 0f;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        transform.position = new Vector3(
            cameraTransform.position.x,
            cameraTransform.position.y + yOffset,
            transform.position.z
        );
    }
}