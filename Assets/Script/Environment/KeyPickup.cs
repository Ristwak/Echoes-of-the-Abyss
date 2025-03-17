using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    private Transform player;
    private bool isInRange = false;
    public bool havekey = false;
    private bool playPickupSound = false;
    private bool playDropSound = false;

    private static KeyPickup currentKeyPickup; // Reference to the currently picked-up key
    private AudioSource audioSource;
    public AudioClip keyPickupSound;
    public AudioClip keyDropSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
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
                PlaySound();
            }
        }
    }

    void PickupObject()
    {
        havekey = true;
        playPickupSound = true;

        // Update the reference in PlayerInteractions
        PlayerInteractions playerInteractions = player.GetComponent<PlayerInteractions>();
        if (playerInteractions != null)
        {
            playerInteractions.UpdateKeyReference(this);
        }

        // Store the currently picked-up key
        currentKeyPickup = this;
        Debug.Log("Playing key pickup sound");

        // Deactivate the key object (Simulating picking up)
        gameObject.SetActive(false);
    }

    void DropPreviousKey()
    {
        if (currentKeyPickup != null)
        {
            // Reactivate the previous key at player's position
            playDropSound = true;
            currentKeyPickup.transform.position = player.position; // Drops key in front of player
            Debug.Log("Playing key drop sound");
            
            currentKeyPickup.gameObject.SetActive(true);

            // Reset havekey for the dropped key
            currentKeyPickup.havekey = false;
        }
    }

    void PlaySound()
    {
        if(playPickupSound)
        {
            AudioSource.PlayClipAtPoint(keyPickupSound, Camera.main.transform.position);
        }
        if(playDropSound)
        {
            AudioSource.PlayClipAtPoint(keyDropSound, Camera.main.transform.position);
            // audioSource.PlayOneShot(keyDropSound);
        }
    }
}
