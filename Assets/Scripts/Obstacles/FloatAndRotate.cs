using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [Header("Float")]
    public float floatAmplitude = 0.25f;   // floating height
    public float floatFrequency = 1.5f;    // floating speed (cycles per second)

    [Header("Rotate")]
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f); // rotation speed in degrees per second (x, y, z)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Fluttuazione
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);

        // Rotazione su se stesso
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
