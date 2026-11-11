using UnityEngine;

public class Lift : MonoBehaviour
{
    [Header("Drag Objects Here")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Animator liftAnimator;

    [Header("Animation Settings")]
    [SerializeField] private string triggerParameterName = "StartLift";

    private void Start()
    {
        // Force the animator to freeze at the start of the game
        if (liftAnimator != null)
        {
            liftAnimator.speed = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerObject == null || liftAnimator == null) return;

        if (other.gameObject == playerObject || other.transform.IsChildOf(playerObject.transform))
        {
            // 1. Parent the player to the lift
            playerObject.transform.SetParent(transform, true);

            // 2. Verify player is a child, then unfreeze and play animation
            if (playerObject.transform.parent == transform)
            {
                liftAnimator.speed = 1f;
                liftAnimator.SetTrigger(triggerParameterName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerObject == null || liftAnimator == null) return;

        if (other.gameObject == playerObject || other.transform.IsChildOf(playerObject.transform))
        {
            // Unparent player and freeze animation immediately
            playerObject.transform.SetParent(null, true);
            liftAnimator.speed = 0f;
        }
    }
}
