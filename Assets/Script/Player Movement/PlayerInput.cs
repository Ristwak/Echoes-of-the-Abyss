using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 5f;
    private bool isGrounded;
    private float jumpForce = 5f;
    private PlayerMovement playerMovement;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = new PlayerMovement();
        playerMovement.Movement.Enable();
        playerMovement.Movement.Jump.performed += Jump;
        playerMovement.Movement.WASD.performed += Movement;
    }

    void Update()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Debug.Log("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
    }
}
