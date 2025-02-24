using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform cameraPoint;
    public float mouseSensitivity = 100f;
    public float maxLookUpAngle = 60f;
    public float maxLookDownAngle = -45f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f); // Default offset

    private float xRotation = 0f;
    private float yRotation = 0f;
    private PlayerInput playerInput;
    private Vector3 desiredCameraPosition;

    void Awake()
    {
        transform.SetParent(cameraPoint); // Attach camera to cameraPoint
        transform.localPosition = cameraOffset; // Apply the offset
        transform.localRotation = Quaternion.identity; // Reset rotation
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput = player.GetComponent<PlayerInput>();
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDownAngle, maxLookUpAngle);

        // Rotate cameraPoint instead of the camera
        cameraPoint.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // Adjust camera position to avoid clipping into the player
        AdjustCameraPosition();
    }

    private void AdjustCameraPosition()
    {
        // Desired camera position (offset from cameraPoint)
        Vector3 targetPosition = cameraPoint.position + cameraPoint.TransformDirection(cameraOffset);

        // Raycast to check for obstacles between the cameraPoint and the desired position
        if (Physics.Linecast(cameraPoint.position, targetPosition, out RaycastHit hit))
        {
            // Move the camera closer if there's an obstacle (e.g., the player body)
            transform.position = hit.point;
        }
        else
        {
            // No obstruction, place the camera at the desired offset
            transform.position = targetPosition;
        }
    }
}
