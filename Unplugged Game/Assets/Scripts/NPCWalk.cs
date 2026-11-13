using UnityEngine;

public class NPCWaypointMover : MonoBehaviour
{
    [Header("Animation")]
    public AnimationClip walkAnimation; 

    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f;

    private Animator anim;
    private int currentPointIndex = 0;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        if (anim != null && walkAnimation != null)
        {
           
            AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            overrideController["walk"] = walkAnimation;
            anim.runtimeAnimatorController = overrideController;

            anim.Play("walk");
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform targetPoint = waypoints[currentPointIndex];

        
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

       
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
        }
    }
}
