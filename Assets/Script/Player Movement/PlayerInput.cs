using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float sideMovementSpeed = 0.6f;
    public float backMovementSpeed = 0.5f;
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
        // Using a raycast slightly below the player
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.1f);

        // Reset jump animation if grounded
        if (isGrounded && isJumping)
        {
            animator.Play(inputVector == Vector2.zero ? "Idle" : "Walking");
            isJumping = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Button Pressed");
        Debug.Log("isGrounded: " + isGrounded);

        if (context.performed && isGrounded) // Fix: Allow jumping only when grounded
        {
            Debug.Log("Jump Executed");
            // animator.Play("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isJumping = true;
            isGrounded = false; // Prevent double jumping
        }
    }

    private void MovePlayer()
    {
        if (isJumping) return; // Prevent movement while jumping

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

        // Adjust speed based on direction
        float moveSpeed = speed;

        if (inputVector.y < 0) // Moving Backward (S)
        {
            moveSpeed = backMovementSpeed; // Reduce speed to 50%
            animator.Play("Walking Backwards");
        }
        else if (inputVector.x < 0) // Right (D)
        {
            moveSpeed = sideMovementSpeed; // Reduce speed to 70%
            Debug.Log("Moving Right");
            animator.Play("Right Walk");
        }
        else if (inputVector.x > 0) // Moving Left (A)
        {
            moveSpeed = sideMovementSpeed; // Reduce speed to 70%
            Debug.Log("Moving Left");
            animator.Play("Left Walk");
        }
        else // Moving Forward (W)
        {
            animator.Play("Walking");
        }

        rb.linearVelocity = new Vector3(desiredMoveDirection.x * moveSpeed, rb.linearVelocity.y, desiredMoveDirection.z * moveSpeed);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Stairs"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Stairs"))
        {
            isGrounded = false;
        }
    }
}
