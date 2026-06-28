using UnityEngine;
using UnityEngine.UI;

public class BarUiManager : SingletonMO<BarUiManager>
{
    [SerializeField] private Image overworkBar;
    [SerializeField] private Image suspicionBar;

    [Header("Danger Feedback")]
    [Tooltip("Ratio (0-1) past which the suspicion bar turns red, pulses, and the heartbeat plays.")]
    [SerializeField, Range(0f, 1f)] private float dangerThreshold = 0.7f;
    [SerializeField] private Color suspicionSafeColor = new Color(1f, 0.85f, 0.2f);
    [SerializeField] private Color suspicionDangerColor = new Color(1f, 0.15f, 0.15f);
    [Tooltip("Looping heartbeat; played while suspicion is in the danger zone. Optional.")]
    [SerializeField] private AudioSource heartbeat;
    [Tooltip("How hard the bar pulses in the danger zone.")]
    [SerializeField] private float pulseAmount = 0.08f;
    [SerializeField] private float pulseSpeed = 6f;

    private Vector3 suspicionBaseScale = Vector3.one;
    private bool capturedBaseScale;

    private void Update()
    {
        if (StatsManager.Instance == null || !GameData.Instance.IsGameActive) return;

        if (!capturedBaseScale && suspicionBar != null)
        {
            suspicionBaseScale = suspicionBar.rectTransform.localScale;
            capturedBaseScale = true;
        }

        float overwork = StatsManager.Instance.OverworkPressureRatio;
        float suspicion = StatsManager.Instance.SuspicionRatio;

        overworkBar.fillAmount = overwork;
        suspicionBar.fillAmount = suspicion;

        ApplyDanger(suspicion);
    }

    private void ApplyDanger(float suspicion)
    {
        if (suspicionBar == null) return;

        // Color ramps continuously from safe (at threshold) to full danger (at max).
        float dangerT = dangerThreshold >= 1f ? 0f :
            Mathf.Clamp01((suspicion - dangerThreshold) / (1f - dangerThreshold));
        suspicionBar.color = Color.Lerp(suspicionSafeColor, suspicionDangerColor, dangerT);

        bool inDanger = suspicion >= dangerThreshold;

        // Pulse harder the closer to max. Unscaled so it keeps beating during slow-mo.
        float pulse = inDanger
            ? 1f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount * dangerT
            : 1f;
        suspicionBar.rectTransform.localScale = suspicionBaseScale * pulse;

        if (heartbeat != null)
        {
            if (inDanger && !heartbeat.isPlaying) heartbeat.Play();
            else if (!inDanger && heartbeat.isPlaying) heartbeat.Stop();
            // Faster, louder beat near max.
            if (heartbeat.isPlaying)
            {
                heartbeat.pitch = Mathf.Lerp(1f, 1.6f, dangerT);
                heartbeat.volume = Mathf.Lerp(0.4f, 1f, dangerT);
            }
        }
    }

    private void OnDisable()
    {
        if (heartbeat != null && heartbeat.isPlaying) heartbeat.Stop();
        if (suspicionBar != null && capturedBaseScale)
            suspicionBar.rectTransform.localScale = suspicionBaseScale;
    }
}
