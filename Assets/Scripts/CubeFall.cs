using UnityEngine;

public class CubeFall : MonoBehaviour
{
    private Rigidbody rb;
    public TimeManager timeManager;

    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        // Check if the cube starts falling
        if (!isFalling && rb.linearVelocity.y < -0.1f)
        {
            isFalling = true;
            timeManager.DoSlowmotion();
        }
    }
}
