using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    [Header("Variables")]
    public float speed = 5f;
    public float sprintSpeed = 15f;
    public float sideMovementSpeed = 0.6f;
    public float backMovementSpeed = 0.5f;
    public float crouchedMovementSpeed = 0.5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    public float groundDistance = 0.9f;
    public bool IsCrouching { get; private set; } = false;
    public GameObject playerHead;
    public GameObject playerTorso;
    public GameObject playerUnderHalfBody;
    public Camera mainCamera;
    public float standingHeight = 1.6f; // Normal standing height
    public float crouchingHeight = 1.0f; // Height when crouched
    public float cameraTransitionSpeed = 5f; // Speed of transition
    public AudioClip walkingSounds;

    private CharacterController characterController;
    private Animator animator;
    private Vector2 inputVector;
    private Vector3 velocity;
    public bool isJumping = false;
    private bool canMove = false;
    private bool isWakingUp = false;
    private AudioSource audioSource;
    public bool isGrounded;
    public bool isCrouched;
    private Coroutine walkingSoundCoroutine;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isCrouched = false;
    }

    void Start()
    {
        StartCoroutine(InitialAnimationDelay());
        isWakingUp = false;
    }

    private IEnumerator InitialAnimationDelay()
    {
        isWakingUp = true;
        animator.Play("Getting Up");
        yield return new WaitForSeconds(8f);
        playerHead.SetActive(false);
        // playerTorso.SetActive(false);
        // playerUnderHalfBody.SetActive(false);
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        if (isWakingUp)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mainCamera.GetComponent<CameraControl>().enabled = false;
        }
        else
        {
            mainCamera.GetComponent<CameraControl>().enabled = true;
            CheckGrounded();
            HandleInput();
            ApplyGravity();
            if (isCrouched)
                CrouchMoving();
            else
                MovePlayer();
        }
    }

    private void HandleInput()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouched)
        {
            Debug.Log("inside SPACE Jumping");
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouched) StartCrouch();
            else StopCrouch();
        }
    }

    private void Jump()
    {
        Debug.Log("Inside Jump function");
        velocity.y = jumpForce;
        isJumping = true;
    }

    private void MovePlayer()
    {
        isGrounded = true;
        if (inputVector == Vector2.zero)
        {
            StopWalkingSound();
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                animator.Play("Idle");
            return;
        }

        PlayWalkingSound(); // Start playing sound when moving

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
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walking Backwards"))
                animator.Play("Walking Backwards");
        }
        else if (inputVector.x > 0)
        {
            moveSpeed = sideMovementSpeed;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Right Walk"))
                animator.Play("Right Walk");
        }
        else if (inputVector.x < 0)
        {
            moveSpeed = sideMovementSpeed;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Left Walk"))
                animator.Play("Left Walk");
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
                animator.Play("Walking");
        }

        characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);
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
        if (inputVector == Vector2.zero)
        {
            StopWalkingSound();
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle Crouching"))
                animator.Play("Idle Crouching");
            return;
        }

        PlayWalkingSound(); // Start playing sound when moving

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
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Crouched Walking Backwards"))
                animator.Play("Crouched Walking Backwards");
        }
        else if (inputVector.x > 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Crouched Walking Right"))
                animator.Play("Crouched Walking Right");
        }
        else if (inputVector.x < 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Crouched Walking Left"))
                animator.Play("Crouched Walking Left");
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Crouched Walking"))
                animator.Play("Crouched Walking");
        }

        characterController.Move(desiredMoveDirection * moveSpeed * Time.deltaTime);
    }

    private void PlayWalkingSound()
    {
        if (walkingSoundCoroutine == null)
        {
            walkingSoundCoroutine = StartCoroutine(LoopWalkingSound());
        }

        audioSource.volume = isCrouched ? 0.5f : 1f;
    }

    private void StopWalkingSound()
    {
        if (walkingSoundCoroutine != null)
        {
            StopCoroutine(walkingSoundCoroutine);
            walkingSoundCoroutine = null;
            audioSource.Stop();
        }
    }


    private IEnumerator LoopWalkingSound()
    {
        while (inputVector != Vector2.zero && isGrounded) // Keep playing while moving
        {
            audioSource.clip = walkingSounds;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        walkingSoundCoroutine = null; // Reset coroutine reference
    }





    private void CheckGrounded()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && isJumping)
        {
            isJumping = false;
            animator.Play(inputVector == Vector2.zero ? "Idle" : "Walking");
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            if (!isJumping)
            {
                velocity.y = -2f; // Small downward force to keep grounded
            }
            else
            {
                velocity.y = jumpForce; // Apply jump force
                isJumping = false; // Reset jumping state after applying jump force
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // Apply gravity while airborne
        }

        characterController.Move(velocity * Time.deltaTime);
    }

}
