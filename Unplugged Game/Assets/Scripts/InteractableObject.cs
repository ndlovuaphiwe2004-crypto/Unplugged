using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Renderer objRenderer;
    private Color originalColor;
    [SerializeField] private Color highlightColor = Color.red;

    protected virtual void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;
        }
    }

    // Add 'virtual' here
    public virtual void HoverStart()
    {
        if (objRenderer != null)
            objRenderer.material.color = highlightColor;
    }

    // Add 'virtual' here
    public virtual void HoverEnd()
    {
        if (objRenderer != null)
            objRenderer.material.color = originalColor;
    }

    // Add 'virtual' here
    public virtual void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }
}