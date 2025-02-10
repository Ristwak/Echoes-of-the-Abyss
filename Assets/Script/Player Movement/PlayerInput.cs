using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    private bool isGrounded;
    public float jumpForce = 5f;
    public float groundDistance = 0.9f;
    private PlayerMovement playerMovement;
    public Transform cameraTransform;
    
    private Vector2 inputVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = new PlayerMovement();
        playerMovement.Movement.Enable();
        playerMovement.Movement.Jump.performed += Jump;
        playerMovement.Movement.WASD.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
        playerMovement.Movement.WASD.canceled += ctx => inputVector = Vector2.zero;
    }

    void Update()
    {
        CheckGrounded();
        MovePlayer();
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Debug.Log("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private void MovePlayer()
    {
        if (inputVector == Vector2.zero) return; // No movement input

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // Convert movement direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDirection.z + right * moveDirection.x;
        rb.linearVelocity = new Vector3(desiredMoveDirection.x * speed, rb.linearVelocity.y, desiredMoveDirection.z * speed);
    }
}
