using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 5f;
    public float sideMovementSpeed = 0.6f;
    public float backMovementSpeed = 0.5f;
    public float crouchedMovementSpeed = 0.5f;
    public float jumpForce = 5f;
    public float groundDistance = 0.9f;
    public bool IsCrouching { get; private set; } = false;
    public GameObject playerHead;
    
    private Rigidbody rb;
    private bool isCrouched;
    private bool isGrounded;
    private Animator animator;
    private Vector2 inputVector;
    private bool isJumping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isCrouched = false;
        playerHead.active = false;
    }

    void Update()
    {
        CheckGrounded();
        HandleInput();
        if (isCrouched)
            CrouchMoving();
        else
            MovePlayer();
    }

    private void HandleInput()
    {
        // Capture movement input
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouched)
        {
            Debug.Log("Jumping");
            Jump();
        }

        // Crouch input
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouched)
        {
            StartCrouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopCrouch();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isJumping = true;
        isGrounded = false;
    }


    private void MovePlayer()
    {
        if (isJumping) return;

        if (inputVector == Vector2.zero)
        {
            animator.Play("Idle");
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // Stops gliding
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

        Vector3 targetVelocity = new Vector3(desiredMoveDirection.x * moveSpeed, rb.linearVelocity.y, desiredMoveDirection.z * moveSpeed);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 10f); // Smooth stop/start
    }

    private void StartCrouch()
    {
        isCrouched = true;
        IsCrouching = true;
        animator.Play("Crouch");
    }

    private void StopCrouch()
    {
        isCrouched = false;
        IsCrouching = false;
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
            animator.Play("Crouched Walking");
        }

        rb.linearVelocity = new Vector3(desiredMoveDirection.x * moveSpeed, rb.linearVelocity.y, desiredMoveDirection.z * moveSpeed);
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
