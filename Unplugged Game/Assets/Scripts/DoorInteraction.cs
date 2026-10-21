using UnityEngine;
using System.Collections;

public class SingleDoorTrigger : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Angle in degrees around local Z-axis. Flip between 90 and -90 to change direction.")]
    [SerializeField] private float openAngle = -90f;
    [SerializeField] private float speed = 5f;

    private bool isPlayerInside = false;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine animateCoroutine;

    void Start()
    {
        // Capture initial closed orientation
        closedRotation = transform.localRotation;

        // Calculate open orientation around local Z-axis
        openRotation = closedRotation * Quaternion.Euler(0f, 0f, openAngle);
    }

    void Update()
    {
        // Detect keypress when player is inside trigger box
        if (isPlayerInside && (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.F)))
        {
            ToggleDoor();
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Stop any running rotation before starting a new one
        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        animateCoroutine = StartCoroutine(RotateTo(targetRotation));
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        // Smoothly rotate until very close to target angle
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.01f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
            yield return null;
        }

        // Snap precisely to target to ensure clean state completion
        transform.localRotation = targetRotation;
        animateCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}