using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera playerCam;
    private float xRotation = 0f;

    [Header("Look Sensitivity")]
    [Range(0.1f, 20f)]
    public float xSensitivity = 0.5f;
    [Range(0.1f, 20f)]
    public float ySensitivity = 0.5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x * xSensitivity * 0.1f;
        float mouseY = lookInput.y * ySensitivity * 0.1f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}