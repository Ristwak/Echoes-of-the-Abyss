using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Variables")]
    public float frontDistance = 2.0f;
    public Transform raycastOrigin;
    public GameObject raycastPoint;

    private Door door;
    private DualDoor dualDoor;
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenDoor();
            DualDoorInteraction();
        }
    }

    void OpenDoor()
    {
        if (!frontline || keyPickup == null || !keyPickup.havekey) return;

        RaycastHit hit;
        Vector3 rayPoint = raycastPoint ? raycastPoint.transform.position : transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(rayPoint, transform.forward, out hit, frontDistance))
        {
            door = hit.transform.GetComponent<Door>();
            if (door != null && hit.collider.CompareTag("Door") && keyPickup.gameObject.CompareTag("Key1"))
            {
                door.doorHandler();
            }
        }
    }

    void DualDoorInteraction()
    {
        if (!frontline) return;

        RaycastHit hit;
        Vector3 rayPoint = raycastPoint ? raycastPoint.transform.position : transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(rayPoint, transform.forward, out hit, frontDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            dualDoor = hit.transform.parent.GetComponent<DualDoor>();
            if (dualDoor == null)
            {
                Debug.LogWarning("MetallicDoor component NOT found on: " + hit.collider.gameObject.name);
                return;
            }

            if (dualDoor != null)
            {
                if (hit.collider.transform.parent.CompareTag("Almirah"))
                {
                    PlayAnim(hit);
                }
                else if (hit.collider.transform.parent.CompareTag("Door4") && keyPickup.gameObject.CompareTag("Key1"))
                {
                    PlayAnim(hit);
                }
            }
        }
    }

    void CheckFront()
    {
        RaycastHit hit;
        Vector3 rayPoint = raycastPoint ? raycastPoint.transform.position : transform.position + Vector3.up * 1.5f;
        frontline = Physics.Raycast(rayPoint, transform.forward, out hit, frontDistance);

        Debug.DrawRay(rayPoint, transform.forward * frontDistance, frontline ? Color.green : Color.red);
    }

    void PlayAnim(RaycastHit hit)
    {
        if (hit.collider.CompareTag("LeftDoor"))
        {
            Debug.Log("Left Door");
            dualDoor.LeftDoor();
        }
        if (hit.collider.CompareTag("RightDoor"))
        {
            Debug.Log("Right Door");
            dualDoor.RightDoor();
        }
    }
}
