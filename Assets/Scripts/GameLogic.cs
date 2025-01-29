using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private string glanceAnimationName = "Glance";

    private bool isPlayerCaught = false;
    private bool isGlancing = false;

    private bool League = true;
    private bool Excel = false;

    private void OnEnable()
    {
        AltTab.OnAltTab += DoAltTab;
    }
    private void OnDisable()
    {
        AltTab.OnAltTab -= DoAltTab;
    }

    private void Update()
    {
        bool isGlancePlaying = IsPlayingAnimation(glanceAnimationName);

        if (isGlancePlaying && !isGlancing)
            StartGlance();
        else if (!isGlancePlaying && isGlancing)
            EndGlance();
    }

    private void StartGlance()
    {
        isGlancing = true;
        CheckForGame();

        if (isPlayerCaught)
            RestartLevel();
    }

    private void CheckForGame()
    {
        if (League)
            isPlayerCaught = true;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EndGlance()
    {
        isGlancing = false;
    }

    private bool IsPlayingAnimation(string animationName)
    {
        return bossAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    void DoAltTab(bool excel, bool league)
    {
        Excel = excel;
        League = league;
    }
}
