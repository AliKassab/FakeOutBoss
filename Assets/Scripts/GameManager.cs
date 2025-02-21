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

    void SwitchScreen(bool excel, bool league)
    {
        Excel = excel;
        League = league;
    }
}
