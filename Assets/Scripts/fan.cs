using UnityEngine;

public class RotateOnZAxis : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Speed of rotation in degrees per second.")]
    public float rotationSpeed = 50f;

    [Tooltip("Direction of rotation. Positive for clockwise, negative for counter-clockwise.")]
    public float direction = 1f;

    void Update()
    {
        // Calculate the rotation for this frame
        float rotationAmount = rotationSpeed * direction * Time.deltaTime;

        // Apply rotation to the Z axis
        transform.Rotate(0, 0, rotationAmount);
    }
}
