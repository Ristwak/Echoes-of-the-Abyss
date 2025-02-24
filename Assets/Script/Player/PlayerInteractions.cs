using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Variables")]
    public float frontDistance = 2.0f;
    public Transform raycastOrigin;
    public GameObject raycastPoint;

    private Door door;
    private MetallicDoor metallicDoor;
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
            MetallicDoorInteraction();
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
            if (door != null && hit.collider.CompareTag("Door1") && keyPickup.gameObject.CompareTag("Key1"))
            {
                door.doorHandler();
            }
        }
    }

    void MetallicDoorInteraction()
    {
        if (!frontline) return;

        RaycastHit hit;
        Vector3 rayPoint = raycastPoint ? raycastPoint.transform.position : transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(rayPoint, transform.forward, out hit, frontDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            metallicDoor = hit.transform.parent.GetComponent<MetallicDoor>();
            if (metallicDoor == null)
            {
                Debug.LogWarning("MetallicDoor component NOT found on: " + hit.collider.gameObject.name);
                return;
            }

            if (metallicDoor != null)
            {
                if (hit.collider.CompareTag("LeftMDoor"))
                {
                    Debug.Log("Left Door");
                    metallicDoor.LeftDoor();
                }
                if (hit.collider.CompareTag("RightMDoor"))
                {
                    Debug.Log("Right Door");
                    metallicDoor.RightDoor();
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
}
