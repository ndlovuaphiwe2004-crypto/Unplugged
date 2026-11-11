using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private Animator liftAnimator;
    [SerializeField] private string triggerParameterName = "StartLift";

    private Transform player;
    private bool isMoving;

    private void OnTriggerEnter(Collider other)
    {
        Transform playerTransform = GetPlayerTransform(other.transform);

        if (playerTransform == null)
        {
            return;
        }

        // Always parent the player when they step onto the lift
        player = playerTransform;
        player.SetParent(transform, true);

        // Only trigger the animation if the lift is not already moving
        if (!isMoving)
        {
            if (liftAnimator != null)
            {
                liftAnimator.SetTrigger(triggerParameterName);
                isMoving = true;
            }
            else
            {
                Debug.LogWarning("Lift Animator is missing from the Inspector!", this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform playerTransform = GetPlayerTransform(other.transform);

        if (playerTransform != null && playerTransform == player)
        {
            player.SetParent(null, true);
            player = null;
        }
    }

    private Transform GetPlayerTransform(Transform current)
    {
        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                return current;
            }
            current = current.parent;
        }
        return null;
    }

    public void ResetLiftMovement()
    {
        isMoving = false;
    }
}
