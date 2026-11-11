using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public Camera playerCamera;
    public float interactionDistance = 0.3f;
    public Transform holdPoint;

    [Header("UI")]
    public TextMeshProUGUI interactionText;
    public Image crosshairImage; // Drag your crosshair dot Image here in the Inspector
    public Color normalCrosshairColor = Color.white;
    public Color hoverCrosshairColor = Color.green;

    public string dropMessage = "Press G to drop";
    public string rotateMessage = "Hold Right Mouse + Move to rotate";
    public string zoomMessage = "Scroll Wheel to zoom";

    [Header("Clue Hint")]
    public string analyseMessage = "Press R to analyse";

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    public float rotationLerpSpeed = 10f;
    private Quaternion targetRotation;

    [Header("Zoom")]
    public float zoomSpeed = 3f;
    public float minZoom = 0.05f;
    public float maxZoom = 1.6f;
    private float currentZoom = 1.3f;
    private float targetZoom = 1.3f;

    [Header("References")]
    public PlayerLook playerLook;

    private Interactable currentInteractable;
    private GameObject heldObject;
    private bool justInteracted = false;
    private Interactable clueInteractable;

    // Tracking original world state and scale for returning / cleanup
    private Vector3 initialPickupPosition;
    private Quaternion initialPickupRotation;
    private Vector3 originalScale;

    // Flag used by OpenSafe to prevent UI overriding
    public bool isNearSafe = false;

    void Update()
    {
        CheckForInteractable();

        if (heldObject != null)
        {
            if (crosshairImage != null) crosshairImage.color = normalCrosshairColor;

            string currentText = dropMessage + "\n" + rotateMessage + "\n" + zoomMessage;
            if (clueInteractable != null && clueInteractable.HasClue())
            {
                currentText += "\n" + analyseMessage;
            }
            interactionText.text = currentText;
            interactionText.gameObject.SetActive(true);

            if (clueInteractable != null && clueInteractable.HasClue() && Input.GetKeyDown(KeyCode.R))
            {
                clueInteractable.OpenInfoPanel(this);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropObject();
                return;
            }

            if (Input.GetMouseButton(1))
            {
                playerLook.enabled = false;

                float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

                targetRotation *= Quaternion.AngleAxis(-rotX, playerCamera.transform.up);
                targetRotation *= Quaternion.AngleAxis(rotY, playerCamera.transform.right);

                heldObject.transform.rotation = Quaternion.Lerp(
                    heldObject.transform.rotation,
                    targetRotation,
                    Time.deltaTime * rotationLerpSpeed
                );
            }

            if (Input.GetMouseButtonUp(1))
            {
                playerLook.enabled = true;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetZoom -= scroll * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }

            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * 12f);

            // Offset relative to holdPoint (Z = 1.3 on the camera)
            Vector3 localTargetPos = new Vector3(0, 0, currentZoom - 1.3f);
            Vector3 worldTargetPos = holdPoint.TransformPoint(localTargetPos);

            Vector3 camPos = playerCamera.transform.position;
            Vector3 direction = worldTargetPos - camPos;
            float distance = direction.magnitude;

            RaycastHit[] hits = Physics.SphereCastAll(camPos, 0.15f, direction.normalized, distance);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject == heldObject || hit.collider.transform.IsChildOf(transform))
                {
                    continue;
                }

                worldTargetPos = hit.point - direction.normalized * 0.15f;
                break;
            }

            heldObject.transform.position = Vector3.Lerp(
                heldObject.transform.position,
                worldTargetPos,
                Time.deltaTime * 15f
            );

            return;
        }

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.F))
        {
            currentInteractable.Interact(this);
            interactionText.gameObject.SetActive(false);
            justInteracted = true;
        }

        if (currentInteractable != null && currentInteractable.infoPanel != null
            && currentInteractable.infoPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                currentInteractable.CloseInfoPanel(this);
            }
        }
    }

    void CheckForInteractable()
    {
        if (heldObject != null)
        {
            currentInteractable = null;
            if (crosshairImage != null) crosshairImage.color = normalCrosshairColor;
            return;
        }

        if (isNearSafe)
        {
            currentInteractable = null;
            if (crosshairImage != null) crosshairImage.color = normalCrosshairColor;
            return;
        }

        currentInteractable = null;

        // Shoot a precise ray straight from the center of the camera (where your crosshair dot is)
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Physics.Raycast automatically hits the closest object directly under the crosshair
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                currentInteractable = interactable;

                // Change crosshair color only when the dot is directly on the interactable object
                if (crosshairImage != null) crosshairImage.color = hoverCrosshairColor;

                if (!justInteracted)
                {
                    interactionText.text = interactable.interactionMessage;
                    interactionText.gameObject.SetActive(true);
                }
                return;
            }
        }

        // Reset crosshair color and hide text when the dot is not on an interactable
        if (crosshairImage != null) crosshairImage.color = normalCrosshairColor;
        interactionText.gameObject.SetActive(false);
        justInteracted = false;
    }

    public void PickUpObject(GameObject objectToPickUp)
    {
        heldObject = objectToPickUp;

        initialPickupPosition = heldObject.transform.position;
        initialPickupRotation = heldObject.transform.rotation;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        heldObject.transform.SetParent(holdPoint, true);

        currentZoom = 1.3f;
        targetZoom = 1.3f;
        heldObject.transform.localPosition = new Vector3(0, 0, currentZoom - 1.3f);

        targetRotation = heldObject.transform.rotation;

        Debug.Log("Picked up: " + heldObject.name);
    }

    void DropObject()
    {
        if (heldObject == null) return;

        if (clueInteractable != null &&
            clueInteractable.infoPanel != null &&
            clueInteractable.infoPanel.activeSelf)
        {
            clueInteractable.CloseInfoPanel(this);
        }

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        heldObject.transform.SetParent(null, true);

        // Always return to original pickup location
        heldObject.transform.position = initialPickupPosition;
        heldObject.transform.rotation = initialPickupRotation;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Returned to initial position: " + heldObject.name);

        heldObject = null;
        clueInteractable = null;

        justInteracted = false;
        currentInteractable = null;

        interactionText.gameObject.SetActive(false);

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = true;

        Time.timeScale = 1f;
        playerLook.enabled = true;
    }

    public void SetClueInteractable(Interactable interactable)
    {
        clueInteractable = interactable;
    }
}