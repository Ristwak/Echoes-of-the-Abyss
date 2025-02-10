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
    private Animator animator;
    private Vector2 inputVector;
    private bool isJumping = false; // Track jump animation state

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);

        // Reset jump animation if grounded
        if (isGrounded && isJumping)
        {
            animator.Play("Idle"); // Reset to idle or walking based on movement
            isJumping = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Outside If condition of Jump");
        if (context.performed && !isGrounded)
        {
            Debug.Log("Inside If condition of Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            animator.Play("Jump");
            isJumping = true;
        }
    }

    private void MovePlayer()
    {
        if (isJumping) return; // Don't override jump animation

        if (inputVector == Vector2.zero)
        {
            animator.Play("Idle");  // Play idle animation when no input
            return;
        }

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // Convert movement direction relative to camera
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDirection.z + right * moveDirection.x;
        rb.linearVelocity = new Vector3(desiredMoveDirection.x * speed, rb.linearVelocity.y, desiredMoveDirection.z * speed);

        animator.Play("Walking"); // Keep walking animation looping
    }
}
