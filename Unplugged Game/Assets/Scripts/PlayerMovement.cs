using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;

    [Header("Movement Settings")]
    public float speed = 3.0f;
    public float crouchSpeed = 1.5f;
    public float gravity = -9.81f;

    [Header("Height Settings")]
    public float standingHeight = 2.0f;
    public float crouchHeight = 1.0f;

    [Header("Camera View Settings")]
    public Transform cameraTransform;

    [Header("Animation Settings")]
    [Tooltip("Crossfade duration in seconds between state switches.")]
    public float animationFadeDuration = 0.15f;

    private float originalCameraY;
    private bool isCrouching = false;
    private string currentAnimationState = "";

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Automatically fetch the Animator attached to this GameObject or its child mesh
        animator = GetComponentInChildren<Animator>();

        // Baseline standing capsule parameters
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

        // Initialize in the Idle state via code
        ChangeAnimationState("Idle");
    }

    void Update()
    {
        HandleCrouchInput();
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
        // 1. Calculate movement speed & direction
        float currentSpeed = isCrouching ? crouchSpeed : speed;
        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(transform.TransformDirection(move) * currentSpeed * Time.deltaTime);

        // 2. Control animation transitions purely via script logic
        bool isMoving = input.magnitude > 0.1f;
        if (isMoving)
        {
            ChangeAnimationState("Walk");
        }
        else
        {
            ChangeAnimationState("Idle");
        }

        // 3. Apply gravity
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Switches the Animator state directly via code using CrossFade.
    /// </summary>
    private void ChangeAnimationState(string newState)
    {
        // Prevent restarting the same animation if it is already playing
        if (currentAnimationState == newState) return;

        if (animator != null)
        {
            animator.CrossFade(newState, animationFadeDuration);
            currentAnimationState = newState;
        }
    }
}