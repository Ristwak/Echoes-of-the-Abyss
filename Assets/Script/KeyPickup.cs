using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupRange = 2f; // The range within which the player can pick up the key
    private Transform player;
    private bool isInRange = false;
    public GameObject playerKey;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Check if player is within pickup range
            isInRange = distance <= pickupRange;

            // Pick up the key when pressing 'E'
            if (isInRange && Input.GetKeyDown(KeyCode.E))
            {
                PickupKey();
            }
        }
    }

    void PickupKey()
    {
        Debug.Log("Key Picked Up!");
        // Destroy(gameObject); // Remove key from scene
        gameObject.active = false;
        playerKey.gameObject.active = true;
    }
}
