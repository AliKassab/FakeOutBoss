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

    // Pause for win/lose screens. Sets timeScale to 0 but restores fixedDeltaTime so
    // a prior slow-mo can't leave it corrupted (0.001) for the next round.
    public void Freeze()
    {
        SetTimeScale(0f);
        Time.fixedDeltaTime = 0.02f;
    }

    public void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
}
