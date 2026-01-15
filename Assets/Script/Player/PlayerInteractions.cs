using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
          [Header("Variables")]
          public float frontDistance = 2.0f;
          public float frontlinetoOpenDoor = 1.5f;

          private MorgueBox morgueBox;
          private Almirah almirah;
          private Door door;
          private DualDoor dualDoor;
          private Rigidbody rb;
          private Animator animator;
          private bool frontline;

          [Header("References")]
          public Transform raycastOrigin;
          public GameObject raycastPoint;
          public KeyPickup keyPickup;
          public FilePickup filePickup;
          public Transform fileInHand;

          void Awake()
          {
                    rb = GetComponent<Rigidbody>();
                    animator = GetComponent<Animator>();
          }

          private bool holdFilePose;

          void Update()
          {
                    CheckFront();

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                              OpenDoor();
                              DualDoorInteraction();
                              checkMorgue();
                              NoKeythings();
                    }

                    if(Input.GetKey(KeyCode.T))
                    {
                        holdFilePose = true;
                    }
                    else
                    {
                        holdFilePose = false;
                    }
          }

          void LateUpdate()
          {
                    if (holdFilePose)
                              fileInHand.gameObject.SetActive(true);
                    else
                              fileInHand.gameObject.SetActive(false);
          }


          void OpenDoor()
          {
                    if (!frontline || keyPickup == null || !keyPickup.havekey)
                    {
                              return;
                    }

                    Transform camTransform = Camera.main.transform;

                    Vector3 rayOrigin = camTransform.position;

                    Vector3 rayDirection = camTransform.forward;

                    frontline = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, frontDistance);

                    if (frontline)
                    {
                              door = hit.transform.GetComponent<Door>();
                              if (door != null)
                              {
                                        if (hit.collider.CompareTag("Door1") && keyPickup.gameObject.CompareTag("Key1"))
                                        {
                                                  door.doorHandler();
                                        }
                                        if (hit.collider.CompareTag("StairDoor") && keyPickup.gameObject.CompareTag("StairKey"))
                                        {
                                                  door.doorHandler();
                                        }
                                        if (hit.collider.CompareTag("NoKeyDoor"))
                                        {
                                                  door.doorHandler();
                                        }
                              }
                    }
          }

          void DualDoorInteraction()
          {
                    if (!frontline) return;

                    Transform camTransform = Camera.main.transform;

                    Vector3 rayOrigin = camTransform.position;

                    Vector3 rayDirection = camTransform.forward;

                    frontline = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, frontDistance);

                    if (frontline)
                    {
                              dualDoor = hit.transform.parent.GetComponent<DualDoor>();
                              Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
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
                                        if (hit.collider.transform.parent.CompareTag("Door4") && keyPickup.gameObject.CompareTag("Key4"))
                                        {
                                                  PlayAnim(hit);
                                        }
                              }
                    }
          }

          void checkMorgue()
          {
                    Transform camTransform = Camera.main.transform;

                    Vector3 rayOrigin = camTransform.position;

                    Vector3 rayDirection = camTransform.forward;

                    frontline = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, frontDistance);

                    if (frontline)
                    {
                              morgueBox = hit.transform.parent.GetComponent<MorgueBox>();
                              if (morgueBox != null)
                              {
                                        morgueBox.PlayAnim(hit.collider.gameObject.name);
                              }
                              else
                              {
                                        Debug.LogWarning("MorgueBox component NOT found on: " + hit.collider.gameObject.name);
                              }
                    }
          }

          void NoKeythings()
          {
                    if (!frontline) return;

                    Transform camTransform = Camera.main.transform;

                    Vector3 rayOrigin = camTransform.position;

                    Vector3 rayDirection = camTransform.forward;

                    frontline = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, frontDistance);

                    if (frontline)
                    {
                              almirah = hit.transform.GetComponentInParent<Almirah>();
                              if (almirah != null)
                              {
                                        if (hit.collider.CompareTag("TableDrawer"))
                                        {
                                                  Debug.Log("Almirah play animation from nokeythings");
                                                  almirah.doorHandler();
                                        }
                              }
                              else
                              {
                                        Debug.LogWarning("Almirah component NOT found on: " + hit.collider.gameObject.name);
                              }
                    }
          }

          void CheckFront()
          {
                    Transform camTransform = Camera.main.transform;

                    Vector3 rayOrigin = camTransform.position;

                    Vector3 rayDirection = camTransform.forward;

                    frontline = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, frontDistance);

                    if (frontline)
                    {
                              Debug.Log("Hit: " + hit.collider.name);
                    }

                    Debug.DrawRay(rayOrigin, rayDirection * frontDistance, frontline ? Color.green : Color.red);
          }

          void PlayAnim(RaycastHit hit)
          {
                    if (hit.collider.CompareTag("LeftDoor"))
                    {
                              dualDoor.LeftDoor();
                    }
                    if (hit.collider.CompareTag("RightDoor"))
                    {
                              dualDoor.RightDoor();
                    }
          }

          public void UpdateKeyReference(KeyPickup newKey)
          {
                    keyPickup = newKey;
          }
          public void UpdateFileReference(FilePickup newFile)
          {
                    filePickup = newFile;
          }
}
 