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

    [Header("Control")]
    public bool canLook = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCam == null && Camera.main != null)
        {
            playerCam = Camera.main;
        }
    }

    public void Look(Vector2 lookInput)
    {
        if (!enabled || !canLook) return;

        float mouseX = lookInput.x * xSensitivity * 0.5f;
        float mouseY = lookInput.y * ySensitivity * 0.5f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCam != null)
        {
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * mouseX);
    }
}