using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer Excel;
    public SpriteRenderer League;

    public static event Action<bool,bool> OnAltTab;

    private void Start()
    {
        GetComponent<Animator>().Play("Typing");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ToggleWindows();
    }

    public void ToggleWindows()
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
