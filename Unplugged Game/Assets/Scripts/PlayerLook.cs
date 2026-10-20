using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerLook : MonoBehaviour
{
    [Header("Camera Reference")]
    public Camera playerCam;

    [Header("Look Sensitivity")]
    [Range(1f, 500f)]
    public float xSensitivity = 100f;
    [Range(1f, 500f)]
    public float ySensitivity = 100f;

    private float xRotation = 0f;

    void Start()
    {
        // Lock mouse cursor to game window center and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Look(Vector2 lookInput)
    {
        // NO Time.deltaTime for Legacy Input.GetAxis!
        float mouseX = lookInput.x * xSensitivity;
        float mouseY = lookInput.y * ySensitivity;

        // Pitch (Up / Down) -> Clamped strictly to Camera local rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        // Apply pitch ONLY to the local camera
        if (playerCam != null)
        {
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Yaw (Left / Right) -> Rotates player body horizontally around Y axis ONLY
        transform.Rotate(Vector3.up * mouseX);
    }
}
