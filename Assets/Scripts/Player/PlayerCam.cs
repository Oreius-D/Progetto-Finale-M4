using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // Flag to skip the first update frame
    private bool initialized = false;

    // Sensitivity settings for mouse movement
    [Header("Mouse Sensitivity")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    // Reference to the player's orientation transform
    [SerializeField] private Transform orientation;

    // Variables to store rotation values
    float xRotation;
    float yRotation;

    private void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize rotation values based on current camera rotation      
        Vector3 angles = transform.rotation.eulerAngles;
        yRotation = angles.y;
        xRotation = angles.x;

        // Adjust xRotation to be in the range [-180, 180]
        if (xRotation > 180f) xRotation -= 360f;
    }

    private void Update()
    {
        // Skip the first update frame to prevent sudden camera movement on start
        if (!initialized)
        {
            initialized = true;
            return;
        }

        // Get mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;
        // Update rotation values based on mouse input
        yRotation += mouseX;
        xRotation -= mouseY;
        // Clamp the vertical rotation to prevent flipping
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // Apply rotations to the camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
