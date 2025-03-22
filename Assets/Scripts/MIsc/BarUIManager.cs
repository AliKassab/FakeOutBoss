using UnityEngine;
using UnityEngine.UI;

public class BarUiManager : MonoBehaviour
{
    [SerializeField] private Image overworkBar;
    [SerializeField] private Image suspicionBar;

    private void Update()
    {
        if (StatsManager.Instance == null || !GameData.Instance.IsGameActive) return;

        overworkBar.fillAmount = StatsManager.Instance.OverworkPressureRatio;
        suspicionBar.fillAmount = StatsManager.Instance.SuspicionRatio;
    }
}