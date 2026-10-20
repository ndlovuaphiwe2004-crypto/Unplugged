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

    private Interactable currentInteractable;
    private GameObject heldObject;
    private bool justInteracted = false;

    void Update()
    {
        CheckForInteractable();

        if (heldObject != null)
        {
            interactionText.text = dropMessage;
            interactionText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropObject();
            }
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
            interactionText.gameObject.SetActive(false);
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

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;

        Debug.Log("Picked up: " + heldObject.name);
    }

    void DropObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            heldObject.transform.SetParent(null);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Dropped: " + heldObject.name);
        heldObject = null;
        interactionText.gameObject.SetActive(false);
    }

}
