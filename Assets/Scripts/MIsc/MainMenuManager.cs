using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : SingletonMO<MainMenuManager>
{
    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button loseScreenGoMenu;
    [SerializeField] Button winScreenGoMenu;

    [Header("Audio")]
    [SerializeField] AudioSource clickAudio;

    [Header("Scene References")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject timeline;
    [SerializeField] GameObject mainMenuCamera;
    [SerializeField] GameObject dayTimerBG;
    [SerializeField] GameObject barsParent;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(Quit);
        loseScreenGoMenu.onClick.AddListener(GoBackToMenu);
        winScreenGoMenu.onClick.AddListener(GoBackToMenu);
        ToggleGameUI(false);
        ToggleCameras(false);
        ToggleGameActivity(false);
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        loseScreenGoMenu.onClick.RemoveAllListeners();
        winScreenGoMenu.onClick.RemoveAllListeners();
    }
    public void PlayGame()
    {
        clickAudio.Play();
        TimeScaleManager.Instance.SetTimeScale(1f);
        ToggleMenuUI(true);
        ToggleCameras(true);
    }

    public void Quit()
        => Application.Quit();

    public void GoBackToMenu()
        => SceneManager.LoadScene(0);

    public void ToggleGameUI(bool IsActive)
    {
        dayTimerBG.SetActive(IsActive);
        barsParent.SetActive(IsActive);
    }

    public void ToggleMenuUI(bool IsActive) => mainMenu.SetActive(!IsActive);

    public void ToggleCameras(bool IsActive)
    {
        mainMenuCamera.SetActive(!IsActive);
        timeline.SetActive(IsActive);
    }

    public void ToggleGameActivity(bool IsActive) 
        => GameData.Instance.IsGameActive = IsActive;

}
