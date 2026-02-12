using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour 
{
    // This script moves an obstacle back and forth between two points (pointA and pointB) at a specified speed, with a wait time at each point.
    [Header("Movement Settings")]
    [SerializeField] private Transform pointA; // Starting point of the obstacle's movement
    [SerializeField] private Transform pointB; // Ending point of the obstacle's movement
    [SerializeField] private float speed = 2f; // Speed at which the obstacle moves between the two points
    [SerializeField] private float waitTime = 0.5f; // Time in seconds that the obstacle waits at each point before moving to the next point

    void Start() 
    {
        // Start the movement coroutine to begin moving the obstacle between the two points
        StartCoroutine(Move()); 
    }

    // Coroutine that handles the movement of the obstacle between pointA and pointB, including waiting at each point
    IEnumerator Move() 
    {
        // Loop indefinitely to keep the obstacle moving back and forth between the two points
        while (true) 
        { 
            yield return MoveTo(pointB.position); // Move towards pointB
            yield return new WaitForSeconds(waitTime); // Wait at pointB for the specified wait time
            yield return MoveTo(pointA.position); // Move back towards pointA
            yield return new WaitForSeconds(waitTime); // Wait at pointA for the specified wait time
        }
    }

    // Coroutine that moves the obstacle towards a target position until it is close enough (within 0.01 units)
    IEnumerator MoveTo(Vector3 target) 
    {
        // Continue moving towards the target position until the distance to the target is less than or equal to 0.01 units
        while (Vector3.Distance(transform.position, target) > 0.01f) 
        {
            // Move the obstacle towards the target position at the specified speed, taking into account the time elapsed since the last frame
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            // Wait until the next frame before continuing to move towards the target position
            yield return null; 
        } 
    } 
}
