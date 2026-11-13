using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;

    [Header("Movement Settings")]
    public float speed = 5.0f; // Increased so movement is obvious
    public float crouchSpeed = 2.5f;
    public float gravity = -9.81f;

    [Header("Height Settings")]
    public float standingHeight = 2.0f;
    public float crouchHeight = 1.0f;

    [Header("Camera View Settings")]
    public Transform cameraTransform;

    [Header("Animation Settings")]
    public float animationFadeDuration = 0.15f;

    private float originalCameraY;
    private bool isCrouching = false;
    private string currentAnimationState = "";

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        controller.height = standingHeight;
        controller.center = new Vector3(0, standingHeight / 2f, 0);

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform != null)
        {
            originalCameraY = cameraTransform.localPosition.y;
        }

        ChangeAnimationState("Idle");
    }

    void Update()
    {
        HandleCrouchInput();

        // Read direct WASD / Arrow keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(moveX, moveZ);

        PlayerMove(input);
    }

    private void HandleCrouchInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                controller.height = crouchHeight;
                controller.center = new Vector3(0, crouchHeight / 2f, 0);

                if (cameraTransform != null)
                {
                    cameraTransform.localPosition = new Vector3(
                        cameraTransform.localPosition.x,
                        originalCameraY * 0.5f,
                        cameraTransform.localPosition.z
                    );
                }
            }
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
                controller.height = standingHeight;
                controller.center = new Vector3(0, standingHeight / 2f, 0);

                if (cameraTransform != null)
                {
                    cameraTransform.localPosition = new Vector3(
                        cameraTransform.localPosition.x,
                        originalCameraY,
                        cameraTransform.localPosition.z
                    );
                }
            }
        }
    }

    public void PlayerMove(Vector2 input)
    {
        float currentSpeed = isCrouching ? crouchSpeed : speed;

        // Calculate direction based on camera orientation or player orientation
        Vector3 move = new Vector3(input.x, 0, input.y);

        // If you want movement relative to where the camera is looking, use cameraTransform:
        Vector3 movementDirection = transform.TransformDirection(move);

        // Move the CharacterController
        controller.Move(movementDirection * currentSpeed * Time.deltaTime);

        // Animations
        bool isMoving = input.sqrMagnitude > 0.01f;
        if (isMoving)
        {
            ChangeAnimationState("Walk");
        }
        else
        {
            ChangeAnimationState("Idle");
        }

        // Gravity
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        if (animator != null)
        {
            animator.CrossFade(newState, animationFadeDuration);
            currentAnimationState = newState;
        }
    }
}