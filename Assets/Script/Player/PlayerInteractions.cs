using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Variables")]
    public float frontDistance = 2.0f;
    public Transform raycastOrigin;
    private Door door;

    private Rigidbody rb;
    private Animator animator;
    public KeyPickup keyPickup;
    private bool frontline;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckFront();
        OpenDoor();
    }

    void OpenDoor()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (keyPickup.havekey && frontline)
            {
                RaycastHit hit;
                Vector3 rayOrigin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1.5f;

                if (Physics.Raycast(rayOrigin, transform.forward, out hit, frontDistance))
                {
                    door = hit.transform.gameObject.GetComponent<Door>();
                    if (hit.collider.CompareTag("Door"))
                    {
                        door.doorHandler();
                    }
                }
            }
        }
    }



    void CheckFront()
    {
        RaycastHit hit;
        Vector3 rayOrigin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1.5f;

        frontline = Physics.Raycast(rayOrigin, transform.forward, out hit, frontDistance);

        Debug.DrawRay(rayOrigin, transform.forward * frontDistance, frontline ? Color.green : Color.red);
    }
}
