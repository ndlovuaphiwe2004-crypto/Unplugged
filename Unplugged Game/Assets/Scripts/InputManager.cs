using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void FixedUpdate()
    {
        // Pass movement input vector to PlayerMovement
        if (playerMovement != null)
        {
            playerMovement.PlayerMove(Player.Movement.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        // Pass look input vector to PlayerLook
        if (playerLook != null)
        {
            playerLook.Look(Player.Look.ReadValue<Vector2>());
        }
    }
}