using UnityEngine;

public class FilePickup : MonoBehaviour
{
    public float pickupRange = 2f;

    private Transform player;
    private bool isInRange = false;

    public bool havefile = false;

    private static FilePickup currentFilePickup;

    public AudioClip filePickupSound;
    public AudioClip fileDropSound;

    public Material fileMaterial;

    // The object shown in the player's hand (assign in Inspector)
    public Transform fileInHand;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (fileInHand != null)
            fileInHand.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= pickupRange;

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Show file in hand
            if (fileInHand != null)
                // fileInHand.gameObject.SetActive(true);

            DropPreviousFile();
            PickupObject();
        }
    }

    void PickupObject()
    {
        havefile = true;

        // Update the reference in PlayerInteractions (if it exists)
        PlayerInteractions playerInteractions = player.GetComponent<PlayerInteractions>();
        if (playerInteractions != null)
            playerInteractions.UpdateFileReference(this);

        currentFilePickup = this;

        // Apply material to the "page" object (child 0 of fileInHand)
        if (fileInHand != null && fileInHand.childCount > 0)
        {
            Renderer pageRenderer = fileInHand.GetChild(0).GetComponent<Renderer>();
            if (pageRenderer != null && fileMaterial != null)
            {
                pageRenderer.material = fileMaterial;
            }
        }

        // Play pickup sound
        if (filePickupSound != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(filePickupSound, Camera.main.transform.position);

        // Hide the pickup object in the world
        gameObject.SetActive(false);
    }

    void DropPreviousFile()
    {
        if (currentFilePickup == null) return;

        // If we're trying to pick up the same one, don't "drop" it
        if (currentFilePickup == this) return;

        // Drop at player's position (slightly in front)
        Vector3 dropPos = player.position + player.forward * 1f;
        currentFilePickup.transform.position = dropPos;

        currentFilePickup.havefile = false;
        currentFilePickup.gameObject.SetActive(true);

        // Play drop sound
        if (fileDropSound != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(fileDropSound, Camera.main.transform.position);
    }
}
