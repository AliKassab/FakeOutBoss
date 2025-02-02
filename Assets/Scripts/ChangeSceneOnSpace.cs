using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneOnSpace : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] AudioSource clickAudio;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
    }
    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
    }
    public void PlayGame()
    {
        clickAudio.Play(); 
        SceneManager.LoadSceneAsync(1); 
    }
}
