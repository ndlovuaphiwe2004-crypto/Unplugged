using UnityEngine;

public class ObjectRotator : InteractableObject
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    private bool isHovered = false;

    public override void HoverStart()
    {
        base.HoverStart();
        isHovered = true;
    }

    public override void HoverEnd()
    {
        base.HoverEnd();
        isHovered = false;
    }

    void Update()
    {
        // Only rotates while the player is aiming at it AND holding the 'R' key
        if (isHovered && Input.GetKey(KeyCode.R))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }
    }
}