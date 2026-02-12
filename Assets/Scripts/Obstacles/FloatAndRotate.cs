using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [Header("Float")]
    public float floatAmplitude = 0.25f;   // floating height
    public float floatFrequency = 1.5f;    // floating speed (cycles per second)

    [Header("Rotate")]
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f); // rotation speed in degrees per second (x, y, z)

    // Starting position for floating calculations
    private Vector3 startPos;

    void Start()
    {
        // Store the initial position to use as the base for floating
        startPos = transform.position;
    }

    void Update()
    {
        // Fluctuate the y position using a sine wave for smooth up-and-down motion
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);

        // Rotate the object around its local axes based on the specified rotation speed
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
