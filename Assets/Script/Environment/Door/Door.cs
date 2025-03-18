using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private bool isDoorOpen;
    private AudioSource audioSource;

    public AudioClip doorSound;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = doorSound;
        isDoorOpen = false;
    }

    public void doorHandler()
    {
        if (animator != null && !isDoorOpen)
        {
            animator.Play("Opening");
            audioSource.Play();
            isDoorOpen = true;
        }
        else
        {
            animator.Play("Closing");
            audioSource.Play();
            isDoorOpen = false;
        }
    }

}
