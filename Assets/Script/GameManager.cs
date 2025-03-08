using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Image fadeImage; // UI Image for blinking effect

    void Start()
    {
        StartCoroutine(BlinkRoutine()); // Start the continuous blinking effect
    }

    private IEnumerator BlinkRoutine()
    {
        while (true) // Infinite loop to keep blinking throughout the game
        {
            yield return new WaitForSeconds(Random.Range(3f, 8f)); // Random wait before next blink
            yield return StartCoroutine(BlinkEffect()); // Perform a blink
        }
    }

    private IEnumerator BlinkEffect()
    {
        yield return StartCoroutine(FadeScreen(1f, 0f, 0.1f)); // Open eyes
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f)); // Small pause for natural effect
        yield return StartCoroutine(FadeScreen(0f, 1f, 0.1f)); // Close eyes
    }

    private IEnumerator FadeScreen(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }
    }
}
