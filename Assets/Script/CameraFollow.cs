using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 2f, -4f); // Position offset from the player
    public float smoothSpeed = 5f; // Camera movement smoothness

    void LateUpdate()
    {
        if (player == null)
            return;

        // Desired position with offset
        Vector3 targetPosition = player.position + player.transform.rotation * offset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Rotate the camera to match the player's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, smoothSpeed * Time.deltaTime);
    }
}
