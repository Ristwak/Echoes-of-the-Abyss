using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform cameraPoint;
    public float mouseSensitivity = 100f;
    public float maxLookUpAngle = 60f;
    public float maxLookDownAngle = -45f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f);

    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool canControl = false; // Prevent player control initially

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.SetParent(cameraPoint);
        transform.localPosition = cameraOffset;
        transform.localRotation = Quaternion.identity;
    }

    void Start()
    {
        Cursor.visible = false;
        Invoke("EnableControl", 8f); // Allow control after 8 seconds
    }

    void LateUpdate()
    {
        if (canControl)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, maxLookDownAngle, maxLookUpAngle);
        }

        // The camera still follows cameraPoint even when input is disabled
        cameraPoint.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);

        AdjustCameraPosition();
    }

    private void AdjustCameraPosition()
    {
        Vector3 targetPosition = cameraPoint.position + cameraPoint.TransformDirection(cameraOffset);

        if (Physics.Linecast(cameraPoint.position, targetPosition, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    private void EnableControl()
    {
        canControl = true; // Enable player control after 8 sec
    }
}
