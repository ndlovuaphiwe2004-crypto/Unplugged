using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Renderer objRenderer;
    private Color originalColor;
    [SerializeField] private Color highlightColor = Color.red;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;
        }
    }

    // Called when the raycast hits this object
    public void HoverStart()
    {
        if (objRenderer != null)
            objRenderer.material.color = highlightColor;
    }

    // Called when the raycast moves off this object
    public void HoverEnd()
    {
        if (objRenderer != null)
            objRenderer.material.color = originalColor;
    }

    // Called when pressing 'E' while aiming at this object
    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }
}