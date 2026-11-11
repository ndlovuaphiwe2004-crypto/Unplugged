using UnityEngine;
using System.Collections;

public class LIFT : MonoBehaviour
{
    [Header("Drag Objects Here")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Animator liftAnimator;

    [Header("Animation Settings")]
    [SerializeField] private string triggerParameterName = "StartLift";
    [SerializeField] private float liftDuration = 5f; // Match your animation length

    private bool isMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (playerObject == null)
        {
            Debug.LogWarning("Drag the Player Object into the Lift Inspector!", this);
            return;
        }

        // Check if the entering collider belongs to the player
        if (other.gameObject == playerObject || other.transform.IsChildOf(playerObject.transform))
        {
            // Step 1: Parent the player to the lift first
            playerObject.transform.SetParent(transform, true);

            // Step 2: Verify the player is actually a child before starting the animation
            if (playerObject.transform.parent == transform && !isMoving)
            {
                if (liftAnimator != null)
                {
                    liftAnimator.SetTrigger(triggerParameterName);
                    isMoving = true;
                    StartCoroutine(ResetLiftAfterDelay());
                }
                else
                {
                    Debug.LogWarning("Lift Animator is missing from the Inspector!", this);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerObject == null) return;

        // Only unparent if the player leaves the trigger
        if (other.gameObject == playerObject || other.transform.IsChildOf(playerObject.transform))
        {
            playerObject.transform.SetParent(null, true);
        }
    }

    private IEnumerator ResetLiftAfterDelay()
    {
        yield return new WaitForSeconds(liftDuration);
        isMoving = false;
    }
}
