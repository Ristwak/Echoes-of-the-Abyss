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
    public float stepHeight = 0.3f;  // Maximum height the player can step up
    public float stepSmooth = 0.2f;  // Speed of smooth stepping
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
        playerHead.SetActive(false);
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
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouched)
        {
            Jump();
        }

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
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
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

        // Apply movement
        Vector3 targetVelocity = new Vector3(desiredMoveDirection.x * moveSpeed, rb.linearVelocity.y, desiredMoveDirection.z * moveSpeed);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 10f);

        // Check for stairs and adjust position
        DetectStairs();
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

    private void DetectStairs()
    {
        // Raycast slightly ahead and downward to detect stairs
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // Slightly above the ground
        Vector3 rayDirection = transform.forward * 0.5f + Vector3.down; // Forward and downward

        if (Physics.Raycast(rayStart, rayDirection, out hit, 0.5f))
        {
            if (hit.collider.CompareTag("Stairs"))
            {
                Vector3 stepUpPosition = transform.position + Vector3.up * stepHeight;
                transform.position = Vector3.Lerp(transform.position, stepUpPosition, stepSmooth);
            }
        }

        Debug.DrawRay(rayStart, rayDirection * 0.5f, Color.blue); // Debug visualization
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
