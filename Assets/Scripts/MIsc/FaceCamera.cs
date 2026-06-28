using UnityEngine;

// Billboards a world-space object (e.g. the boss "!" alert) toward the active camera
// so it stays readable from any angle. Unscaled-independent — just orientation.
public class FaceCamera : MonoBehaviour
{
    private Transform cam;

    private void LateUpdate()
    {
        if (cam == null)
        {
            if (Camera.main == null) return;
            cam = Camera.main.transform;
        }
        // Face away from camera so the text reads correctly (not mirrored).
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
    }
}
