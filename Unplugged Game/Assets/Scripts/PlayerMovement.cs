using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 3.0f;
    public float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void PlayerMove(Vector2 input)
    {
        // Horizontal movement
        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // small downward force to keep grounded
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}