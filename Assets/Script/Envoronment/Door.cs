using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>(); // Ensure the door has an Animator
    }

    public void doorOpening()
    {
        if (animator != null)
        {
            animator.Play("DoorOpening");
            Debug.Log("Door animation triggered via Animator!");
        }
        else
        {
            Debug.LogError("Animator not found on door!");
        }
    }
}
