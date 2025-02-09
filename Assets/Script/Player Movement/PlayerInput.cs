using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody jump;

    void Awake()
    {
        jump = GetComponent<Rigidbody>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump");
            jump.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
