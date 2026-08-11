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

    // Awake runs before OnEnable, so initialization must happen here
    void Awake()
    {
        playerInput = new PlayerInput();
        Player = playerInput.Player;
        playerMovement = GetComponent<PlayerMovement>();
        playerLook = GetComponent<PlayerLook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerMovement.PlayerMove(Player.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        playerLook.Look(Player.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        Player.Enable();
    }

    private void OnDisable()
    {
        Player.Disable();
    }
}