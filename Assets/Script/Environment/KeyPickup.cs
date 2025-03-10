using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupRange = 2f; // The range within which the player can pick up the key
    private Transform player;
    private bool isInRange = false;
    public bool havekey = false;

    private static KeyPickup currentKeyPickup; // Reference to the current key being held

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
                DropPreviousKey(); // Drop the previous key before picking up a new one
                PickupObject();
            }
        }
    }

    void PickupObject()
    {
        havekey = true;

        // Update the reference in PlayerInteractions
        PlayerInteractions playerInteractions = player.GetComponent<PlayerInteractions>();
        if (playerInteractions != null)
        {
            playerInteractions.UpdateKeyReference(this);
        }

        // Store the currently picked-up key
        currentKeyPickup = this;

        // Deactivate the key object (Simulating picking up)
        gameObject.SetActive(false);
    }

    void DropPreviousKey()
    {
        if (currentKeyPickup != null)
        {
            // Reactivate the previous key at player's position
            currentKeyPickup.transform.position = player.position + Vector3.forward * 1f; // Drops key in front of player
            currentKeyPickup.gameObject.SetActive(true);

            // Reset havekey for the dropped key
            currentKeyPickup.havekey = false;
        }
    }
}
