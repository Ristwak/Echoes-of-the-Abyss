using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float rotationSmoothTime = 0.05f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f);
    public float crouchOffsetY = 0.6f; // Camera moves down when crouching
    public float crouchOffsetX = 0.6f; // Camera moves forward when crouching
    public float maxLookUpAngle = 60f;
    public float maxLookDownAngle = -45f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 currentVelocity = Vector3.zero;
    private PlayerInput playerInput;
    private float targetCameraHeight;
    private float targetCameraXOffset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput = player.GetComponent<PlayerInput>();
        targetCameraHeight = cameraOffset.y; // Default height
        targetCameraXOffset = cameraOffset.x; // Default X offset
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDownAngle, maxLookUpAngle);

        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = targetRotation;
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);

        AdjustCameraForCrouch();

        Vector3 targetPosition = player.position + player.TransformDirection(new Vector3(targetCameraXOffset, targetCameraHeight, cameraOffset.z));
        transform.position = targetPosition;
    }

    private void AdjustCameraForCrouch()
    {
        if (playerInput.IsCrouching)
        {
            targetCameraHeight = cameraOffset.y - crouchOffsetY;
            targetCameraXOffset = cameraOffset.x + crouchOffsetX;
        }
        else
        {
            targetCameraHeight = cameraOffset.y;
            targetCameraXOffset = cameraOffset.x;
        }
    }
}
