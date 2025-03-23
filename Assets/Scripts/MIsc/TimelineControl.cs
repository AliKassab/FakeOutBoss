using UnityEngine;

public class TimelineControl : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera.SetActive(true);
        MainMenuManager.Instance.ToggleGameActivity(true);
        MainMenuManager.Instance.ToggleGameUI(true);
    }

}
