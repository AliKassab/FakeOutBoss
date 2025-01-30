using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnSpace : MonoBehaviour
{
    [SerializeField] Camera menuCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject canvas2;
    [SerializeField] GameObject timer;

    //[SerializeField] private KeyChallengeManager keyChallengeManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCamera = Camera.main;
            menuCamera.enabled = false;
            canvas.SetActive(false);
            canvas2.SetActive(true);
            timer.SetActive(true);
            //keyChallengeManager.StartKeyChallenge();

        }
    }
}
