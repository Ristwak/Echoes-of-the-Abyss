using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSystem : MonoBehaviour
{
    private Rigidbody jump;

    void Awake()
    {
        jump = GetComponent<Rigidbody>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump" + context.phase);
        jump.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
