using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.PlayerActions Player;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;

    void Awake()
    {
        playerInput = new PlayerInput();
        Player = playerInput.Player;

        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponent<PlayerLook>();

        if (playerMovement == null)
            Debug.LogError("PlayerMovement component missing on this GameObject");
        if (playerLook == null)
            Debug.LogError("PlayerLook component missing on this GameObject");
    }

    void OnEnable()
    {
        Player.Enable();
    }

    void OnDisable()
    {
        Player.Disable();
    }

    // Moved from FixedUpdate to Update so CharacterController movement is smooth
    void Update()
    {
        if (playerMovement != null)
        {
            playerMovement.PlayerMove(Player.Movement.ReadValue<Vector2>());
        }
    }

    void LateUpdate()
    {
        if (playerLook != null)
        {
            playerLook.Look(Player.Look.ReadValue<Vector2>());
        }
    }
}