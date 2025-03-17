using UnityEngine;

public class TimeScaleManager : SingletonMO<TimeScaleManager>
{
    [SerializeField] private float SlowDownFactor = 0.05f;

    public void DoSlowmotion()
    {
        SetTimeScale(SlowDownFactor);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ResetTime()
    {
        SetTimeScale(1f);
        Time.fixedDeltaTime = 0.02f;  
    }

    public void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
}
