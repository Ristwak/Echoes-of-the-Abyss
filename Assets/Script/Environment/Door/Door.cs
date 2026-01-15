using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;
    private bool isDoorOpen;
    private AudioSource audioSource;

    [Header("Audio")]
    public AudioClip doorSound;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
            Debug.LogError("Animator missing on Door!", this);

        if (audioSource == null)
            Debug.LogError("AudioSource missing on Door!", this);
        else
            audioSource.clip = doorSound;

        playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput == null)
            Debug.LogWarning("PlayerInput not found in scene!");

        isDoorOpen = false;
    }

    public void doorHandler()
    {
        if (animator == null) return;

        if (!isDoorOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        animator.Play("Opening");

        if (playerInput != null)
        {
            Transform player = playerInput.transform;

            // Push player slightly back in WORLD space
            player.LeanMove(player.position - player.forward, 0.3f);
        }

        PlaySound();
        isDoorOpen = true;
    }

    void CloseDoor()
    {
        animator.Play("Closing");
        PlaySound();
        isDoorOpen = false;
    }

    void PlaySound()
    {
        if (audioSource != null && doorSound != null)
            audioSource.Play();
    }
}
