using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer excelScreenSpr;
    [SerializeField] private SpriteRenderer leagueScreenSpr;

    private void Start()
    {
        GetComponent<Animator>().Play("Typing");
        UpdateGameData();
    }

    private void Update()
    {
        if (!GameData.Instance.IsGameActive) return;
        if (!GameData.Instance.IsAILooking && Input.GetKeyDown(KeyCode.Space))
            ToggleWindows();
    }

    public void ToggleWindows()
    {
        ToggleVisibility(excelScreenSpr);
        ToggleVisibility(leagueScreenSpr);
        UpdateGameData();
    }

    private void UpdateGameData() => GameData.Instance.IsPlaying = leagueScreenSpr.enabled;

    private void ToggleVisibility(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}
