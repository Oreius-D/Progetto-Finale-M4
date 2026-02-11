using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target; // The target the camera will follow

    private void LateUpdate()
    {
        // Ensure the camera follows the target's position
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
