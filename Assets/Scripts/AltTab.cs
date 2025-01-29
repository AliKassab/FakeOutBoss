using System;
using UnityEngine;

public class AltTab : MonoBehaviour
{
    public SpriteRenderer Excel;
    public SpriteRenderer League;

    public static event Action<bool,bool> OnAltTab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ToggleWindows();
    }

    private void ToggleWindows()
    {
        ToggleVisibility(Excel);
        ToggleVisibility(League);
        OnAltTab.Invoke(Excel.enabled, League.enabled);
    }

    private void ToggleVisibility(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}
