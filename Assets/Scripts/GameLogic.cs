using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private string glanceAnimationName = "Glance";

    private bool isPlayerCaught = false;

    public bool League = true;
    public bool Excel = false;

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
        bool isLooking = (bossAnimator.gameObject.GetComponent<BossAI>().currentAction == BossAI.Action.Looking);
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

<<<<<<< Updated upstream
    void DoAltTab(bool excel, bool league)
=======
    private void EndGlance()
    {
        isGlancing = false;
    }

    private bool IsPlayingAnimation(string animationName)
    {
        return bossAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public void DoAltTab(bool excel, bool league)
>>>>>>> Stashed changes
    {
        Excel = excel;
        League = league;
    }
}
