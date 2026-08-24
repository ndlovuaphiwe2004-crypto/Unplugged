using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Renderer objRenderer;
    private Color originalColor;
    [SerializeField] private Color highlightColor = Color.red;

    private void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;
        }
    }

    public void HoverStart()
    {
        if (objRenderer != null)
            objRenderer.material.color = highlightColor;
    }

    public void HoverEnd()
    {
        if (objRenderer != null)
            objRenderer.material.color = originalColor;
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }
}