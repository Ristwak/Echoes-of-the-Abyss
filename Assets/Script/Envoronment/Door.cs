using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private bool isDoorOpen;

    void Awake()
    {
        animator = GetComponent<Animator>();
        isDoorOpen = false;
    }

    public void doorHandler()
    {
        if (animator != null && !isDoorOpen)
        {
            animator.Play("DoorOpening");
            Debug.Log("Playing DoorOpening");
            isDoorOpen = true;
        }
        else
        {
            animator.Play("DoorClosing");
            isDoorOpen = false;
        }
    }

}
