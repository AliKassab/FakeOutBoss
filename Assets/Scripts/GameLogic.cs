using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private string glanceAnimationName = "Glance";

    private bool isPlayerCaught = false;

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

    void DoAltTab(bool excel, bool league)
    {
        Excel = excel;
        League = league;
    }
}
