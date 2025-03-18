using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;
    private bool isDoorOpen;
    private AudioSource audioSource;

    public AudioClip doorSound;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerInput = FindObjectOfType<PlayerInput>();
        audioSource.clip = doorSound;
        isDoorOpen = false;
    }

    public void doorHandler()
    {
        if (animator != null && !isDoorOpen)
        {
            animator.Play("Opening");
            playerInput.gameObject.transform.LeanMoveZ (playerInput.gameObject.transform.position.z - 1f, 1f);
            playerInput.gameObject.transform.LeanMoveLocalX (playerInput.gameObject.transform.position.x - 1f, 1f);
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
