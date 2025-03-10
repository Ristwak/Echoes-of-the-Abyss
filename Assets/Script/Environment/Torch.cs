using UnityEngine;

public class Torch : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform playerTorch;
    public float pickupRange = 2f;
    private Transform player;
    private bool isInRange = false;
    public bool havetorch = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Check if player is within pickup range
            isInRange = distance <= pickupRange;

            // Pick up the key when pressing 'E'
            if (isInRange && Input.GetKeyDown(KeyCode.E))
            {
                Pickupobject();
            }

            if (havetorch)
            {
                playerTorch.transform.gameObject.SetActive(true);
                havetorch = true;
            }
        }
    }

    void Pickupobject()
    {
        gameObject.SetActive(false);
        playerTorch.transform.gameObject.SetActive(true);
        havetorch = true;
    }
}
