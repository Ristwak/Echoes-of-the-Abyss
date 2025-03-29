using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public LightFlicker flickeringLight;
    public AudioClip[] cryingSound;
    public AudioClip whisperSound;
    private AudioSource audioSource;

    public float minCryingVolume = 0.5f;
    public float maxCryingVolume = 1f;
    public float minWhisperVolume = 0.5f;
    public float maxWhisperVolume = 1f;
    public float minInterval = 2f;  // Minimum delay between sounds
    public float maxInterval = 5f;  // Maximum delay between sounds

    private bool isPlaying = false;

    void Awake()
    {
        flickeringLight.GetComponent<LightFlicker>().enabled = false;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        if (!isPlaying && !playerInput.isWalking)
        {
            StartCoroutine(PlaySoundsSequentially());
        }
    }

    IEnumerator PlaySoundsSequentially()
    {
        isPlaying = true;

        // Play Crying Sound
        AudioClip cryingClip = cryingSound[Random.Range(0, cryingSound.Length)];
        audioSource.clip = cryingClip;
        audioSource.volume = Random.Range(minCryingVolume, maxCryingVolume);
        audioSource.Play();

        // Wait for the crying sound to finish + a small interval
        yield return new WaitForSeconds(cryingClip.length + Random.Range(minInterval, maxInterval));

        // Play Whisper Sound
        audioSource.clip = whisperSound;
        audioSource.volume = Random.Range(minWhisperVolume, maxWhisperVolume);
        audioSource.Play();

        // Wait for the whisper sound to finish + another interval
        yield return new WaitForSeconds(whisperSound.length + Random.Range(minInterval, maxInterval));

        isPlaying = false; // Allow the sequence to restart
    }
}
