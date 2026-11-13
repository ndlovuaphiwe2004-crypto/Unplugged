using UnityEngine;

public class CarFloatingLoop : MonoBehaviour
{
    public Transform startPoint;   
    public Transform endPoint;     
    public float speed = 5f;       
    public float heightOffset = 1.5f; 

    void Start()
    {
        
        if (startPoint != null)
        {
            transform.position = startPoint.position + new Vector3(0, heightOffset, 0);
        }
    }

    void Update()
    {
        if (startPoint == null || endPoint == null) return;

        
        Vector3 targetEndPosition = endPoint.position + new Vector3(0, heightOffset, 0);
        Vector3 targetStartPosition = startPoint.position + new Vector3(0, heightOffset, 0);

        
        transform.position = Vector3.MoveTowards(transform.position, targetEndPosition, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, targetEndPosition) < 0.001f)
        {
            
            transform.position = targetStartPosition;
        }
    }
}
