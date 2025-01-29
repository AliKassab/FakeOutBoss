using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnSpace : MonoBehaviour
{
    [SerializeField] Camera menuCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject timer;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCamera = Camera.main;
            menuCamera.enabled = false;
            canvas.SetActive(false);
            timer.SetActive(true);

        }
    }
}
