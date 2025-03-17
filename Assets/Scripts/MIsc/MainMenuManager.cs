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
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject mainMenuCamera;
    [SerializeField] GameObject DayTimer;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(Quit);
        loseScreenGoMenu.onClick.AddListener(GoBackToMenu);
        winScreenGoMenu.onClick.AddListener(GoBackToMenu);
        ToggleGameState(false, false);
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        loseScreenGoMenu.onClick.RemoveAllListeners();
        winScreenGoMenu.onClick.RemoveAllListeners();
    }
    private void PlayGame()
        => ToggleGameState(true);
    private void Quit()
        => Application.Quit();

    public void GoBackToMenu()
        => SceneManager.LoadScene(0);

    private void ToggleGameState(bool IsActive, bool ShouldPlayAudio = true)
    {
        if (ShouldPlayAudio)
            clickAudio.Play();

        TimeScaleManager.Instance.SetTimeScale(1f);
        mainMenu.SetActive(!IsActive);
        mainMenuCamera.SetActive(!IsActive);
        playerCamera.SetActive(IsActive);
        DayTimer.SetActive(IsActive);
        GameData.Instance.IsGameActive = IsActive;

    }
}
