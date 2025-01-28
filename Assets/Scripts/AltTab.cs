using UnityEngine;

public class AltTab : MonoBehaviour
{
    public static SpriteRenderer Excel { get; private set; }
    public static SpriteRenderer League { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ToggleWindows();
    }

    private void ToggleWindows()
    {
        ToggleVisibility(Excel);
        ToggleVisibility(League);
    }

    private void ToggleVisibility(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}
