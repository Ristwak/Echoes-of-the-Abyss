using UnityEngine;

public class Almirah : MonoBehaviour
{
    public Animator animator;
    private bool isDoorOpen;

    void Awake()
    {
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
