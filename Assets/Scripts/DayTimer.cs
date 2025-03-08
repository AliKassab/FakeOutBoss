using UnityEngine;
using TMPro;

public class DayTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float currentTime;
    [SerializeField] GameObject WinScreen;

    private int hours;
    private int minutes;

    // Update is called once per frame
    void Update() => EvaluateTime();
    private void EvaluateTime()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            SetTimeText();
        }
        else if (currentTime <= 0)
            WinDay();
    }
    private void SetTimeText()
    {
        hours = Mathf.FloorToInt(currentTime / 60);
        minutes = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{00:00}:{1:00}", hours, minutes);
    }
    private void WinDay()
    {
        WinScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
