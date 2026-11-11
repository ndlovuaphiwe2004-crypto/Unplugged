using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;

    [Header("Movement Settings")]
    public float speed = 3.0f;
    public float gravity = -9.81f;

    [Header("Height Settings")]
    public float standingHeight = 2.0f;

    [Header("Camera View Settings")]
    public Transform cameraTransform;

    [Header("Animation Settings")]
    [Tooltip("Crossfade duration in seconds between state switches.")]
    public float animationFadeDuration = 0.15f;

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

   
        ChangeAnimationState("Idle");
    }

    public void PlayerMove(Vector2 input)
    {
   
        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);

        bool isMoving = input.magnitude > 0.1f;
        if (isMoving)
        {
            ChangeAnimationState("Walk");
        }
        else
        {
            ChangeAnimationState("Idle");
        }

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