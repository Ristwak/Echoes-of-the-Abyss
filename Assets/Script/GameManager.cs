using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public LightFlicker flickeringLight;
    public AudioClip[] cryingSounds;
    public AudioClip whisperSound;
    private AudioSource audioSource;

    public float minCryingVolume = 0.5f;
    public float maxCryingVolume = 1f;
    public float minWhisperVolume = 0.5f;
    public float maxWhisperVolume = 1f;
    public float minInterval = 2f;
    public float maxInterval = 5f;

    private bool isPlaying = false;

    void Awake()
    {
        if (flickeringLight != null)
        {
            flickeringLight.enabled = false; // Ensure it's initially disabled
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure an AudioSource exists
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        playerInput = FindObjectOfType<PlayerInput>();

        if (cryingSounds.Length > 0 && whisperSound != null)
        {
            StartCoroutine(PlaySoundsSequentially());
        }
        else
        {
            Debug.LogError("Audio clips are not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (!isPlaying && Random.value > 0.99f)  // 1% chance per frame to trigger hallucinations
        {
            StartCoroutine(PlaySoundsSequentially());
        }
    }

    IEnumerator PlaySoundsSequentially()
    {
        isPlaying = true;

        // Activate flickering light randomly
        if (flickeringLight != null && Random.value > 0.7f) // 30% chance
        {
            flickeringLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(3f, 7f)); // Flickering duration
            flickeringLight.enabled = false;
        }

        // Play Crying Sound
        if (cryingSounds.Length > 0)
        {
            AudioClip cryingClip = cryingSounds[Random.Range(0, cryingSounds.Length)];
            audioSource.clip = cryingClip;
            audioSource.volume = Random.Range(minCryingVolume, maxCryingVolume);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();

            yield return new WaitForSeconds(cryingClip.length + Random.Range(minInterval, maxInterval));
        }

        // Play Whisper Sound
        if (whisperSound != null)
        {
            audioSource.volume = Random.Range(minWhisperVolume, maxWhisperVolume);
            audioSource.PlayOneShot(whisperSound); // Use PlayOneShot instead of PlayClipAtPoint
            yield return new WaitForSeconds(whisperSound.length + Random.Range(minInterval, maxInterval));
        }

        isPlaying = false;
    }
}
