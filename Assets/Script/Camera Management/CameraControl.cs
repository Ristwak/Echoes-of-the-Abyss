using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float rotationSmoothTime = 0.05f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
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
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.rotation = Quaternion.Euler(0f, yRotation, 0f);
        transform.position = player.position;
    }
}
