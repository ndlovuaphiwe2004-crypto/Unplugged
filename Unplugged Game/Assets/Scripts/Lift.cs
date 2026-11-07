using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float speed = 3f;

    private Transform player;
    private int currentTargetIndex = 0;
    private Transform targetPoint;
    private bool isMoving;

    private void Start()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            // Start at the first waypoint
            transform.position = waypoints[0].position;
            currentTargetIndex = 0;
            SetNextTarget();
        }
    }

    private void Update()
    {
        if (!isMoving || targetPoint == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            transform.position = targetPoint.position;
            isMoving = false; // Stop the lift completely until triggered again
            SetNextTarget();  // Queue up the next destination index
        }
    }

    private void SetNextTarget()
    {
        if (waypoints != null && waypoints.Count > 1)
        {
            // Advance to next index and loop back to 0 if at the end
            currentTargetIndex = (currentTargetIndex + 1) % waypoints.Count;
            targetPoint = waypoints[currentTargetIndex];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react if it's the Player, the lift isn't currently moving, and we have enough waypoints
        if (!other.CompareTag("Player") || isMoving || waypoints == null || waypoints.Count < 2)
        {
            return;
        }

        player = other.transform;
        player.SetParent(transform, true);
        isMoving = true; // Start moving to the pre-calculated target point
    }

    private void OnTriggerExit(Collider other)
    {
        // Unparent the player only if they actually step off while the lift is stopped
        if (other.transform == player && !isMoving)
        {
            player.SetParent(null, true);
            player = null;
        }
    }
}
