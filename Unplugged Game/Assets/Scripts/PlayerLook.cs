using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera playerCam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x * xSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * ySensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
