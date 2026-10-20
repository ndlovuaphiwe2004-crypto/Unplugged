using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
//using UnityEditor.ShaderGraph.Internal;
=======
>>>>>>> origin/Ndlovu
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera playerCam;
    private float xRotation = 0f;
<<<<<<< HEAD
=======

    [Header("Look Sensitivity")]
    [Range(0.1f, 20f)]
    public float xSensitivity = 0.5f;
    [Range(0.1f, 20f)]
    public float ySensitivity = 0.5f;
>>>>>>> origin/Ndlovu

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    void Start()
    {
<<<<<<< HEAD

    }

    void Update()
    {

=======
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
>>>>>>> origin/Ndlovu
    }

    public void Look(Vector2 lookInput)
    {
<<<<<<< HEAD
        float mouseX = lookInput.x * xSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * ySensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
=======
        float mouseX = lookInput.x * xSensitivity * 0.1f;
        float mouseY = lookInput.y * ySensitivity * 0.1f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

>>>>>>> origin/Ndlovu
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}