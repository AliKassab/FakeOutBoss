using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
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

    [Header("Challenge Settings")]
    [SerializeField] int requiredCorrectPresses = 3;
    [SerializeField] Vector2 popUpSize = new Vector2(100, 100);
    [SerializeField] float fadeDuration = 0.5f;
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

    void Update()
    {
        if (!ShouldProcessInput()) return;

        if (!isChallengeActive)
        {
            StartKeyChallenge();
            return;
        }

        if (Input.anyKeyDown)
            HandleKeyPress();

        if (GameData.Instance.IsSpotted)
            FailChallenge();
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
        TimeScaleManager.Instance.DoSlowmotion();
        globalVolume.SetActive(true);
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
        TimeScaleManager.Instance.ResetTime();
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
            keyText.text = key;
        else
            Debug.LogError("Key popup prefab missing TextMeshProUGUI component!");

        StartFadeEffect(currentKeyPopup.AddComponent<CanvasGroup>());
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
        TimeScaleManager.Instance.ResetTime();
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