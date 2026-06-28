using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class KeyChallengeManager : SingletonMO<KeyChallengeManager>
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] GameObject keyPopUpPrefab;
    [SerializeField] RectTransform keyCanvas;
    [SerializeField] GameObject globalVolume;
    [SerializeField] AudioSource keyPressSound;
    [SerializeField] GameObject failScreen;
    [Tooltip("Radial Image (fill type) that drains as the catch window closes. Optional.")]
    [SerializeField] Image challengeTimerRing;

    [Header("Challenge Settings")]
    [SerializeField] int requiredCorrectPresses = 3;
    [SerializeField] Vector2 popUpSize = new Vector2(100, 100);
    [SerializeField] float fadeDuration = 0.5f;

    [Header("Juice")]
    [Tooltip("Ring color when the catch window is full -> empty.")]
    [SerializeField] Color ringSafeColor = new Color(0.3f, 1f, 0.4f);
    [SerializeField] Color ringDangerColor = new Color(1f, 0.2f, 0.2f);
    [Tooltip("Scale punch on a correct key press.")]
    [SerializeField] float correctPopScale = 1.4f;
    #endregion

    #region Private Variables
    string currentKey = "";
    GameObject currentKeyPopup;
    int correctPresses;
    bool isChallengeActive;
    Coroutine fadeCoroutine;
    #endregion

    #region Unity Lifecycle
    void Start() => failScreen.SetActive(false);

    private void OnEnable()
        => StatsManager.Instance.OnSuspicionMax += FailChallenge;

    private void OnDisable()
        => StatsManager.Instance.OnSuspicionMax -= FailChallenge;

    void Update()
    {
        if (!ShouldProcessInput()) return;

        if (!isChallengeActive)
        {
            StartKeyChallenge();
            return;
        }

        UpdateTimerRing();

        if (Input.anyKeyDown)
            HandleKeyPress();

        if (GameData.Instance.IsSpotted)
            FailChallenge();
    }

    void UpdateTimerRing()
    {
        if (challengeTimerRing == null) return;
        float t = GameData.Instance.LookProgress;
        challengeTimerRing.fillAmount = t;
        challengeTimerRing.color = Color.Lerp(ringDangerColor, ringSafeColor, t);
    }
    #endregion

    #region Public Methods
    public void ByPassChallenge()
    {
        PassChallenge();
    }
    #endregion

    #region Challenge Logic
    void StartKeyChallenge()
    {
        isChallengeActive = true;
        correctPresses = 0;
        // Clear any stale spot flag so a previous round can't instant-fail this one.
        GameData.Instance.IsSpotted = false;
        TimeScaleManager.Instance.DoSlowmotion();
        globalVolume.SetActive(true);
        if (challengeTimerRing != null)
        {
            challengeTimerRing.gameObject.SetActive(true);
            challengeTimerRing.fillAmount = 1f;
        }
        GenerateNewKey();
    }

    void HandleKeyPress()
    {
        if (IsMouseInput()) return;

        if (Input.GetKeyDown(currentKey.ToLower()))
        {
            keyPressSound.Play();
            correctPresses++;

            if (correctPresses >= requiredCorrectPresses)
                PassChallenge();
            else
                GenerateNewKey();
        }
        else
        {
            // Wrong key = busted. Slam the feedback before the fail resolves.
            if (ScreenJuice.Instance != null) { ScreenJuice.Instance.Shake(); ScreenJuice.Instance.Flash(); }
            GameData.Instance.IsSpotted = true;
        }
    }

    void PassChallenge()
    {
        CleanupChallenge();
        PlayerController.Instance.ToggleWindows();
    }

    void FailChallenge()
    {
        failScreen.SetActive(true);
        GameData.Instance.IsGameActive = false;
        // Freeze (not raw SetTimeScale(0)): also restores fixedDeltaTime so the
        // slow-mo value doesn't leak into the next round and crawl the bars.
        TimeScaleManager.Instance.Freeze();
    }
    #endregion

    #region Key Generation
    void GenerateNewKey()
    {
        CleanupExistingKey();
        currentKey = GetRandomKey();
        DisplayKey(currentKey, CalculateRandomPosition());
    }

    string GetRandomKey() => GameData.Instance.ChallengeCharacters[Random.Range(0, GameData.Instance.ChallengeCharacters.Length)].ToString();

    Vector2 CalculateRandomPosition()
    {
        Rect canvasRect = keyCanvas.rect;
        return new Vector2(
            Random.Range(-canvasRect.width / 2 + popUpSize.x, canvasRect.width / 2 - popUpSize.x),
            Random.Range(-canvasRect.height / 2 + popUpSize.y, canvasRect.height / 2 - popUpSize.y)
        );
    }
    #endregion

    #region UI Management
    void DisplayKey(string key, Vector2 position)
    {
        currentKeyPopup = Instantiate(keyPopUpPrefab, keyCanvas);
        currentKeyPopup.GetComponent<RectTransform>().anchoredPosition = position;

        if (currentKeyPopup.TryGetComponent(out TextMeshProUGUI keyText))
            keyText.text = key.ToUpper();
        else
            Debug.LogError("Key popup prefab missing TextMeshProUGUI component!");

        StartFadeEffect(currentKeyPopup.AddComponent<CanvasGroup>());
        StartCoroutine(PopInRoutine(currentKeyPopup.transform));
    }

    void StartFadeEffect(CanvasGroup canvasGroup)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvas(canvasGroup, 0, 1));
    }

    IEnumerator FadeCanvas(CanvasGroup group, float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        group.alpha = endAlpha;
    }
    #endregion

    #region Helper Methods
    bool ShouldProcessInput() =>
        GameData.Instance.IsGameActive &&
        GameData.Instance.IsPlaying &&
        GameData.Instance.IsAILooking;

    bool IsMouseInput()
    {
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ||
               Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) ||
               Input.GetKeyDown(KeyCode.Mouse2) || Input.GetKeyDown(KeyCode.Mouse3);
    }

    void CleanupChallenge()
    {
        CleanupExistingKey();
        isChallengeActive = false;
        GameData.Instance.IsAILooking = false;
        GameData.Instance.IsPlaying = false;
        globalVolume.SetActive(false);
        if (challengeTimerRing != null) challengeTimerRing.gameObject.SetActive(false);
        TimeScaleManager.Instance.ResetTime();
    }

    // Scale-in pop so each new key punches onto screen instead of just appearing.
    IEnumerator PopInRoutine(Transform t)
    {
        if (t == null) yield break;
        float dur = 0.14f, e = 0f;
        while (e < dur && t != null)
        {
            // Overshoot 0 -> correctPopScale -> 1, unscaled so it reads during slow-mo.
            float k = e / dur;
            float s = Mathf.LerpUnclamped(0f, correctPopScale, Mathf.Sin(k * Mathf.PI * 0.5f));
            if (k > 0.6f) s = Mathf.Lerp(correctPopScale, 1f, (k - 0.6f) / 0.4f);
            t.localScale = Vector3.one * s;
            e += Time.unscaledDeltaTime;
            yield return null;
        }
        if (t != null) t.localScale = Vector3.one;
    }

    void CleanupExistingKey()
    {
        if (currentKeyPopup == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        Destroy(currentKeyPopup);
        currentKeyPopup = null;
    }
    #endregion
}