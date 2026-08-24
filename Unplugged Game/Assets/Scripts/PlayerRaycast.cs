using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 10f;
    private InteractableObject currentTarget;

    void Update()
    {
       
        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);

        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

        InteractableObject foundTarget = null;

        
        foreach (RaycastHit hit in hits)
        {
            InteractableObject target = hit.collider.GetComponent<InteractableObject>();

            
            if (target == null)
            {
                target = hit.collider.GetComponentInParent<InteractableObject>();
            }

            if (target != null)
            {
                foundTarget = target;
                break; 
            }
        }

        
        if (foundTarget != currentTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.HoverEnd();
            }

            currentTarget = foundTarget;

            if (currentTarget != null)
            {
                currentTarget.HoverStart();
            }
        }

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact();
        }
    }
}