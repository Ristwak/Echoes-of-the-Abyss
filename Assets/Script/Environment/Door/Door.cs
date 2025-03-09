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
            animator.Play("Opening");
            Debug.Log("Playing DoorOpening");
            isDoorOpen = true;
        }
        else
        {
            animator.Play("Closing");
            isDoorOpen = false;
        }
    }

}
