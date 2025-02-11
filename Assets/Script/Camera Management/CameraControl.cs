using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float rotationSmoothTime = 0.05f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f); // Camera follows behind
    public float cameraTiltAmount = 5f; // Adjusts how much the camera tilts
    public float fixedXRotation = 10f; // Fixed vertical angle of the camera

    private float yRotation = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Get mouse input for horizontal rotation only
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;

        // Apply fixed vertical angle and allow horizontal rotation
        Quaternion targetRotation = Quaternion.Euler(fixedXRotation, yRotation, 0f);
        transform.rotation = targetRotation;
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);  // Player rotates with camera

        // Smoothly move camera to follow the player with an offset
        Vector3 targetPosition = player.position + player.TransformDirection(cameraOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, rotationSmoothTime);
    }
}