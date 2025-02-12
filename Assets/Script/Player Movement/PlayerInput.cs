using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float sideMovementSpeed = 0.6f;
    public float backMovementSpeed = 0.5f;
    public float crouchedMovementSpeed = 0.5f;
    private bool isGrounded;
    private bool isCrouched;
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
        playerMovement.Movement.Crouch.started += StartCrouch;
        playerMovement.Movement.Crouch.canceled += StopCrouch;

        playerMovement.Movement.WASD.started += ctx => inputVector = ctx.ReadValue<Vector2>();
        playerMovement.Movement.WASD.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
        playerMovement.Movement.WASD.canceled += ctx => inputVector = Vector2.zero;

        isCrouched = false;
    }

    void Update()
    {
        CheckGrounded();
        if (isCrouched)
            CrouchMoving();
        else
            MovePlayer();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.1f);

        if (isGrounded && isJumping)
        {
            animator.Play(inputVector == Vector2.zero ? "Idle" : "Walking");
            isJumping = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isCrouched)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            animator.Play("Jump");
            isJumping = true;
            isGrounded = false;
        }
    }

    private void MovePlayer()
    {
        if (isJumping) return;

        if (inputVector == Vector2.zero)
        {
            animator.Play("Idle");
            return;
        }

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDirection.z + right * moveDirection.x;

        float moveSpeed = speed;

        if (inputVector.y < 0)
        {
            moveSpeed = backMovementSpeed;
            animator.Play("Walking Backwards");
        }
        else if (inputVector.x < 0)
        {
            moveSpeed = sideMovementSpeed;
            animator.Play("Right Walk");
        }
        else if (inputVector.x > 0)
        {
            moveSpeed = sideMovementSpeed;
            animator.Play("Left Walk");
        }
        else
        {
            animator.Play("Walking");
        }

        rb.linearVelocity = new Vector3(desiredMoveDirection.x * moveSpeed, rb.linearVelocity.y, desiredMoveDirection.z * moveSpeed);
    }

    private void StartCrouch(InputAction.CallbackContext context)
    {
        isCrouched = true;
    }

    private void StopCrouch(InputAction.CallbackContext context)
    {
        isCrouched = false;
        animator.Play("Idle");
    }

    private void CrouchMoving()
    {
        if (isJumping) return;

        if (inputVector == Vector2.zero)
        {
            animator.Play("Idle Crouching");
            return;
        }

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveDirection.z + right * moveDirection.x;
        float moveSpeed = crouchedMovementSpeed;

        // Set animation based on movement direction
        if (inputVector.y < 0)
        {
            animator.Play("Crouched Walking Backwards");
        }
        else if (inputVector.x > 0)
        {
            animator.Play("Crouched Walking Right");
        }
        else if (inputVector.x < 0)
        {
            animator.Play("Crouched Walking Left");
        }
        else
        {
            animator.Play("Crouched Walking Backwards");
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
