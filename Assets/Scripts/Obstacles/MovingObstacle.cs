using UnityEngine;
using System.Collections;
public class MovingObstacle : MonoBehaviour 
{ 
    public Transform pointA; 
    public Transform pointB; 
    public float speed = 2f; 
    public float waitTime = 0.5f; 
    
    void Start() 
    { 
        StartCoroutine(Move()); 
    } 
    
    IEnumerator Move() 
    { 
        while (true) 
        { 
            yield return MoveTo(pointB.position); 
            yield return new WaitForSeconds(waitTime); 
            yield return MoveTo(pointA.position); 
            yield return new WaitForSeconds(waitTime); 
        }
    } 
    
    IEnumerator MoveTo(Vector3 target) 
    { 
        while (Vector3.Distance(transform.position, target) > 0.01f) 
        { 
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime); 
            yield return null; 
        } 
    } 
}
