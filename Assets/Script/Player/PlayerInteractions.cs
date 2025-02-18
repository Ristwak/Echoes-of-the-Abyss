using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Variables")]
    public float frontDistance = 2.0f;
    public Transform raycastOrigin;
    public bool openingDoor;
    public Door door;

    private Rigidbody rb;
    private Animator animator;
    public KeyPickup keyPickup;
    private bool frontline;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        openingDoor = false;
    }

    void Update()
    {
        CheckFront();
        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenDoor();
            openingDoor = true;
        }
    }

    void OpenDoor()
    {
        Debug.Log("Into OpenDoor Function");

        if (keyPickup.havekey && frontline)
        {
            RaycastHit hit;
            Vector3 rayOrigin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1.5f;

            if (Physics.Raycast(rayOrigin, transform.forward, out hit, frontDistance))
            {
                Debug.Log("Into OpenDoor's Keypickup Function");

                if (hit.collider.CompareTag("Door"))
                {
                    Debug.Log("Door detected: " + hit.collider.gameObject.name);

                    // ✅ Assign the door reference dynamically
                    door = hit.collider.GetComponent<Door>();

                    if (door != null)
                    {
                        door.doorOpening(); // ✅ Play animation from the Door script
                        Debug.Log("Door animation triggered!");
                    }
                    else
                    {
                        Debug.LogError("Door script not found on the hit object!");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Conditions not met: Have key? " + keyPickup?.havekey + " | Frontline? " + frontline);
        }
    }



    void CheckFront()
    {
        RaycastHit hit;
        Vector3 rayOrigin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1.5f; // ✅ Use mid-level

        frontline = Physics.Raycast(rayOrigin, transform.forward, out hit, frontDistance);

        // Debugging visualization
        Debug.DrawRay(rayOrigin, transform.forward * frontDistance, frontline ? Color.green : Color.red);
    }
}
