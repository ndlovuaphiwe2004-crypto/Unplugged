using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
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

    private float originalCameraY;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        
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
    }

    void Update()
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

        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(transform.TransformDirection(move) * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}