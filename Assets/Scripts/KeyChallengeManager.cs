using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem.LowLevel;

public class KeyChallengeManager : MonoBehaviour
{
    public GameObject KeyPopUp;
    public RectTransform KeyCanvas;
    public TimeScaleManager TimeScaleManager;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject globalVolume;
    [SerializeField] private AudioSource keyPressSound;
    [SerializeField] private GameObject Fail;

    private string currentKey = "";
    private GameObject currentKeyPopup;
    private Vector2 randomPosition;
    private int correctPresses = 0;
    private bool isChallengeActive = false;

    private void Start()
    {
        Fail.SetActive(false);
    }

    private void Update()
    {
        if (!GameData.Instance.IsGameActive) return;
        if (GameData.Instance.IsPlaying && GameData.Instance.IsAILooking)
        {
            if (!isChallengeActive)
            {
                StartKeyChallenge();
                return;
            }
            
            if (Input.anyKeyDown)
                CheckKeyPress();
        }
    }

    public void StartKeyChallenge()
    {
        isChallengeActive = true;
        correctPresses = 0;
        SetNewKey();
        TimeScaleManager.DoSlowmotion();
        globalVolume.SetActive(true);
    }

    public void ByPassChallenge()
    {
        DestroyExistingKey();
        EndKeyChallenge();
        playerController.ToggleWindows();
    }

    private void CheckKeyPress()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ||
        Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2) ||
        Input.GetKeyDown(KeyCode.Mouse3) || Input.GetKeyDown(KeyCode.Mouse4))
            return;

        if (Input.GetKeyDown(currentKey.ToLower()))
        {
            keyPressSound.Play();
            DestroyExistingKey();
            SetNewKey();
            correctPresses++;
            if (correctPresses >= 3)
            {
                DestroyExistingKey();
                EndKeyChallenge();
                playerController.ToggleWindows();
            }
        }
        else
        {
            EndKeyChallenge();
            FailChallenge();
        }
    }
    private void SetNewKey()
    {
        currentKey = GetRandomKey();
        randomPosition = GenerateRandomPosition();
        DisplayKey(currentKey, randomPosition);
    }
    private void FailChallenge()
    {
        Fail.SetActive(true);
        GameData.Instance.IsGameActive = false;
        Time.timeScale = 0f;
    }
    private string GetRandomKey()
    {
        string keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return keys[Random.Range(0, keys.Length)].ToString(); 
    }

    private Vector2 GenerateRandomPosition()
    {
        RectTransform canvasRect = KeyCanvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        Vector2 letterSize = KeyPopUp.GetComponent<RectTransform>().sizeDelta;

        float xMin = -canvasWidth / 2 + letterSize.x / 2;
        float xMax = canvasWidth / 2 - letterSize.x / 2;
        float yMin = -canvasHeight / 2 + letterSize.y / 2;
        float yMax = canvasHeight / 2 - letterSize.y / 2;

        float xPos = Random.Range(xMin, xMax);
        float yPos = Random.Range(yMin, yMax);

        return new Vector2(xPos, yPos);
    }

    private void DisplayKey(string key, Vector2 position)
    {
        currentKeyPopup = Instantiate(KeyPopUp, KeyCanvas);

        TextMeshProUGUI keyText = currentKeyPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (keyText != null)
            keyText.text = key;
        else
            Debug.LogWarning("TextMeshProUGUI component missing in key popup prefab!");

        RectTransform rectTransform = currentKeyPopup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;

        CanvasGroup canvasGroup = currentKeyPopup.AddComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasIn(canvasGroup));
    }

    private void DestroyExistingKey()
    {
        if (currentKeyPopup != null)
        {
            StopAllCoroutines(); 
            Destroy(currentKeyPopup);
            currentKeyPopup = null;
        }
    }

    private void EndKeyChallenge()
    {
        isChallengeActive = false;
        GameData.Instance.IsAILooking = false;
        GameData.Instance.IsPlaying = false;
        globalVolume.SetActive(false);
        TimeScaleManager.ResetTime();
    }

    private IEnumerator FadeCanvasIn(CanvasGroup canvasGroup)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (canvasGroup == null || canvasGroup.gameObject == null)
            {
                yield break;
            }

            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }


        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
    }
}
