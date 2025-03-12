using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAdvancer : MonoBehaviour
{
    [SerializeField] Button? playButton;
    [SerializeField] AudioSource? clickAudio;
    [SerializeField] Button? quitButton;

    private void OnEnable()
    {
        playButton?.onClick.AddListener(PlayGame);
        quitButton?.onClick.AddListener(Quit);
    }
    private void OnDisable()
    {
        playButton?.onClick.RemoveAllListeners();
        quitButton?.onClick.RemoveAllListeners();
    }
    public void PlayGame()
    {
        clickAudio?.Play();
        GameData.Instance.IsGameActive = true;
        SceneManager.LoadSceneAsync(1); 
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void GoBackToMenu()
    {
        clickAudio?.Play();
        Time.timeScale = 1f;
        GameData.Instance.IsGameActive = false;
        SceneManager.LoadSceneAsync(0);
    }
}
