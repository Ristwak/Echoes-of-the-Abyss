using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public LightFlicker flickeringLight;

    void Awake()
    {
        flickeringLight.GetComponent<LightFlicker>().enabled = false;
    }
}
