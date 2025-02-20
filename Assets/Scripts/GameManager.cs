using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator aiBrainAnimator;

    public bool League = true;
    public bool Excel = false;

    private void OnEnable()
    {
        PlayerController.OnAltTab += SwitchScreen;
    }
    private void OnDisable()
    {
        PlayerController.OnAltTab -= SwitchScreen;
    }

    private void Update()
    {
        bool isLooking = (aiBrainAnimator.gameObject.GetComponent<AiBrain>().currentAction == AiBrain.Action.Looking);
        if (isLooking)
            CheckForGame();
    }

    private void CheckForGame()
    {
        if (League)
            RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SwitchScreen(bool excel, bool league)
    {
        Excel = excel;
        League = league;
    }
}
