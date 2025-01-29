using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    [SerializeField]    TextMeshProUGUI timerText;
    [SerializeField]    float rTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rTime > 0)
        {
            rTime -= Time.deltaTime;
        }
        else if (rTime <0){
            rTime = 0;
        }

        int hours = Mathf.FloorToInt(rTime / 60);
        int minutes = Mathf.FloorToInt(rTime % 60);
        timerText.text = string.Format("{00:00}:{1:00}",hours,minutes);
    }
   
}
