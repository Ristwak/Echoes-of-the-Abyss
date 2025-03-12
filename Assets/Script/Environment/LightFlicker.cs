using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light[] flickeringLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);
    }

    void Flicker()
    {
        foreach (var item in flickeringLight)
        {
            item.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
