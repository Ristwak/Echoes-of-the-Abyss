using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Transform spawnPoint;
    void Start()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
