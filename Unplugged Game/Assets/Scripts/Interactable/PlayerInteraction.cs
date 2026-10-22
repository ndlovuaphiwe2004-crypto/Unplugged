using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public Camera playerCamera;
    public float interactionDistance = 0.3f;
    public Transform holdPoint;

    [Header("UI")]
    public TextMeshProUGUI interactionText;
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
    public float zoomSpeed = 0.5f;
    public float minZoom = 1.2f;
    public float maxZoom = 1.5f;
    private float currentZoom = 0.5f;
    private float targetZoom = 0.5f;

    [Header("References")]
    public PlayerLook playerLook;

    private Interactable currentInteractable;
    private GameObject heldObject;
    private bool justInteracted = false;
    private Interactable clueInteractable;

    // NEW: Flag used by OpenSafe to prevent UI overriding
    public bool isNearSafe = false;

    void Update()
    {
        CheckForInteractable();

        if (heldObject != null)
        {
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

            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * 8f);

            Vector3 localTargetPos = new Vector3(0, 0, -currentZoom);
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
                Time.deltaTime * 10f
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
            return;
        }

        // If near the safe, let OpenSafe control the text display
        if (isNearSafe)
        {
            currentInteractable = null;
            return;
        }

        currentInteractable = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, interactionDistance);

        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                Vector3 dirToObject = (hit.collider.transform.position - playerCamera.transform.position).normalized;
                float dot = Vector3.Dot(playerCamera.transform.forward, dirToObject);

                if (dot > 0.5f)
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        currentInteractable = interactable;

                        if (!justInteracted)
                        {
                            interactionText.text = interactable.interactionMessage;
                            interactionText.gameObject.SetActive(true);
                        }
                        return;
                    }
                }
            }
        }

        interactionText.gameObject.SetActive(false);
        justInteracted = false;
    }

    public void PickUpObject(GameObject objectToPickUp)
    {
        heldObject = objectToPickUp;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        heldObject.transform.SetParent(holdPoint, true);
        currentZoom = 0.5f;
        targetZoom = 0.5f;
        heldObject.transform.localPosition = new Vector3(0, 0, -currentZoom);

        targetRotation = heldObject.transform.rotation;

        Debug.Log("Picked up: " + heldObject.name);
    }

    void DropObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            heldObject.transform.SetParent(null, true);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Dropped: " + heldObject.name);
        heldObject = null;
        clueInteractable = null;
        interactionText.gameObject.SetActive(false);

        playerLook.enabled = true;
    }

    public void SetClueInteractable(Interactable interactable)
    {
        clueInteractable = interactable;
    }
}