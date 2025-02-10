using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float rotationSmoothTime = 0.05f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f); // Camera follows behind
    public float cameraTiltAmount = 5f; // Adjusts how much the camera tilts

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate player left/right
        yRotation += mouseX;

        // Apply rotation to camera and player
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = targetRotation;
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);  // Player rotates with camera

        // Add slight tilt to camera when moving left/right
        float tilt = Input.GetAxis("Horizontal") * cameraTiltAmount;
        transform.rotation *= Quaternion.Euler(0, 0, -tilt); // Tilts the camera left/right

        // Smoothly move camera to follow the player with an offset
        Vector3 targetPosition = player.position + player.TransformDirection(cameraOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, rotationSmoothTime);
    }
}
