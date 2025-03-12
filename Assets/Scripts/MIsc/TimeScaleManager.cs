using UnityEngine;

public class TimeScaleManager : SingletonMO<TimeScaleManager>
{
    public float SlowDownFactor = 0.05f;
    public float SlowDownTime = 2f;

    public void DoSlowmotion()
    {
        Time.timeScale = SlowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;  
        Time.fixedDeltaTime = 0.02f;  
    }
}
