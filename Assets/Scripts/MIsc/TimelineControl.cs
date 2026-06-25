using UnityEngine;

public class TimelineControl : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Guarantee normal time the moment gameplay actually begins, regardless of
        // what a previous win/lose left behind.
        TimeScaleManager.Instance.ResetTime();
        playerCamera.SetActive(true);
        MainMenuManager.Instance.ToggleGameActivity(true);
        MainMenuManager.Instance.ToggleGameUI(true);
    }

}
