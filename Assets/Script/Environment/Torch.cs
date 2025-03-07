using UnityEngine;

public class Torch : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera transform

    void Start()
    {
        if (cameraTransform == null)
        {
            // Try to find the main camera if not assigned
            cameraTransform = Camera.main?.transform;
        }
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            // Sync the torch's rotation with the camera's rotation
            transform.rotation = cameraTransform.rotation;
        }
    }
}
