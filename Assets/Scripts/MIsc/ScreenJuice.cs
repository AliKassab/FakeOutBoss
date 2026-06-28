using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Camera shake + full-screen flash for impact feedback (wrong key, etc.).
// All timing is UNSCALED so it still reads during the slow-mo QTE.
public class ScreenJuice : SingletonMO<ScreenJuice>
{
    [Header("Shake")]
    [Tooltip("Camera that gets shaken. Defaults to Camera.main if left empty.")]
    [SerializeField] private Transform shakeTarget;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.25f;

    [Header("Flash")]
    [Tooltip("Full-screen UI Image (start disabled / alpha 0). Optional.")]
    [SerializeField] private Image flashImage;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.45f);
    [SerializeField] private float flashFadeSpeed = 4f;

    private Vector3 shakeBaseLocalPos;
    private Coroutine shakeRoutine;
    private float flashAlpha;

    protected override void Awake()
    {
        base.Awake();
        ResolveShakeTarget();
    }

    // Camera.main can be the menu camera at Awake; re-resolve to the active one when needed.
    private void ResolveShakeTarget()
    {
        if (shakeTarget == null && Camera.main != null) shakeTarget = Camera.main.transform;
    }

    private void Update()
    {
        if (flashImage == null) return;
        // Fade the flash out independent of timeScale.
        if (flashAlpha > 0f)
        {
            flashAlpha = Mathf.Max(0f, flashAlpha - Time.unscaledDeltaTime * flashFadeSpeed);
            var c = flashColor; c.a = flashAlpha;
            flashImage.color = c;
            flashImage.enabled = flashAlpha > 0.001f;
        }
    }

    public void Flash()
    {
        if (flashImage == null) return;
        flashAlpha = flashColor.a;
    }

    public void Shake() => Shake(shakeMagnitude, shakeDuration);

    public void Shake(float magnitude, float duration)
    {
        ResolveShakeTarget();
        if (shakeTarget == null) return;
        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(ShakeRoutine(magnitude, duration));
    }

    private IEnumerator ShakeRoutine(float magnitude, float duration)
    {
        shakeBaseLocalPos = shakeTarget.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float falloff = 1f - (elapsed / duration);
            Vector2 offset = Random.insideUnitCircle * magnitude * falloff;
            shakeTarget.localPosition = shakeBaseLocalPos + new Vector3(offset.x, offset.y, 0f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        shakeTarget.localPosition = shakeBaseLocalPos;
        shakeRoutine = null;
    }
}
