using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
        if (GameData.Instance.IsPlaying && GameData.Instance.IsAILooking && GameData.Instance.IsAlmostSpotted)
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
        GameData.Instance.IsAlmostSpotted = true;
        isChallengeActive = true;
        correctPresses = 0;
        currentKey = GetRandomKey();
        randomPosition = GenerateRandomPosition();
        DisplayKey(currentKey, randomPosition);
        TimeScaleManager.DoSlowmotion();
        globalVolume.SetActive(true);
    }



    private void CheckKeyPress()
    {
        if (Input.GetKeyDown(currentKey.ToLower()))
        {
            keyPressSound.Play();
            currentKey = GetRandomKey();
            randomPosition = GenerateRandomPosition();
            DestroyExistingKey();
            DisplayKey(currentKey, randomPosition);
            correctPresses++;
            if (correctPresses >= 3)
            {
                DestroyExistingKey();
                EndKeyChallenge();
                playerController.ToggleWindows();
                correctPresses = 0;
            }
        }
        else
        {
            DestroyExistingKey();
            EndKeyChallenge();
            FailChallenge();
        }
    }

    private void FailChallenge()
    {
        Fail.SetActive(true);
        Time.timeScale = 0f;
    }
    private string GetRandomKey()
    {
        string keys = "abcdefghijklmnopqrstuvwxyz";
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
        GameData.Instance.IsAlmostSpotted = false;
        GameData.Instance.IsAILooking = false;
        isChallengeActive = false;
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
