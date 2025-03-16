using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player;
    private CameraControl cameraControl;
    public float maxDistance = 5f;
    public float moveDistance = 0.4f;
    public LayerMask collisionLayers; // Layers the camera should collide with
    private bool canControl = false;

    private Vector3 defaultCameraOffset;

    void Start()
    {
        defaultCameraOffset = transform.localPosition.normalized * maxDistance;
        cameraControl = GetComponentInParent<CameraControl>();
        defaultCameraOffset = cameraControl.cameraOffset;
        Invoke("EnableControl", 8f); // Allow control after 8 seconds
    }

    void LateUpdate()
    {
        if(!canControl) return;

        Vector3 desiredPosition = player.position + player.TransformDirection(defaultCameraOffset);
        RaycastHit hit;

        // Check if something is between the player and the camera
        if (Physics.Raycast(player.position, (desiredPosition - player.position).normalized, out hit, maxDistance, collisionLayers))
        {
            cameraControl.cameraOffset.z = moveDistance;
        }
        else
        {
            cameraControl.cameraOffset.z = defaultCameraOffset.z;
        }
    }

    private void EnableControl()
    {
        canControl = true; // Enable player control after 8 sec
    }
}
