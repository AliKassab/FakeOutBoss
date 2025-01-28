using System.Collections;
using TMPro;
using UnityEngine;

public class KeyChallengeManager : MonoBehaviour
{
    public GameObject keyPopupPrefab;
    public Transform canvasTransform;
    public TextMeshProUGUI resultText;
    private bool challengeActive = false;
    private string currentKey = "";
    private GameObject currentKeyPopup;

    private Vector2 randomPosition;
    public TimeManager timeManager;

    private void Start()
    {
        StartKeyChallenge();
    }

    private void Update()
    {
        if (challengeActive && Input.anyKeyDown)
        {
            CheckKeyPress();
        }
    }

    private void StartKeyChallenge()
    {
        challengeActive = true;
        currentKey = GenerateRandomKey();
        randomPosition = GenerateRandomPosition();
        DisplayKeyPopup(currentKey, randomPosition);
        timeManager.DoSlowmotion();
    }

    private void CheckKeyPress()
    {
        if (Input.GetKeyDown(currentKey.ToLower()))
        {
            currentKey = GenerateRandomKey();
            randomPosition = GenerateRandomPosition();

            DestroyOldKeyPopup();
            DisplayKeyPopup(currentKey, randomPosition);
        }
        else
        {
            EndKeyChallenge(false);
        }
    }

    private string GenerateRandomKey()
    {
        string keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return keys[Random.Range(0, keys.Length)].ToString(); 
    }

    private Vector2 GenerateRandomPosition()
    {
        RectTransform canvasRect = canvasTransform.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        Vector2 letterSize = keyPopupPrefab.GetComponent<RectTransform>().sizeDelta;

        float xMin = -canvasWidth / 2 + letterSize.x / 2;
        float xMax = canvasWidth / 2 - letterSize.x / 2;
        float yMin = -canvasHeight / 2 + letterSize.y / 2;
        float yMax = canvasHeight / 2 - letterSize.y / 2;

        float xPos = Random.Range(xMin, xMax);
        float yPos = Random.Range(yMin, yMax);

        return new Vector2(xPos, yPos);
    }

    private void DisplayKeyPopup(string key, Vector2 position)
    {
        currentKeyPopup = Instantiate(keyPopupPrefab, canvasTransform);

        TextMeshProUGUI keyText = currentKeyPopup.GetComponentInChildren<TextMeshProUGUI>();
        if (keyText != null)
        {
            keyText.text = key;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component missing in key popup prefab!");
        }

        RectTransform rectTransform = currentKeyPopup.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;

        CanvasGroup canvasGroup = currentKeyPopup.AddComponent<CanvasGroup>();
        StartCoroutine(FadeIn(canvasGroup));
    }

    private void DestroyOldKeyPopup()
    {
        if (currentKeyPopup != null)
        {
            StopAllCoroutines(); 
            Destroy(currentKeyPopup);
            currentKeyPopup = null;
        }
    }


    private void EndKeyChallenge(bool success)
    {
        challengeActive = false;

        resultText.text = success ? "Success!" : "Failed!";
        resultText.gameObject.SetActive(true);

        // Destroy key pop-ups (clear the canvas)
        //foreach (Transform child in canvasTransform)
        //{
        //    Destroy(child.gameObject);
        //}

        // Reset time after a short delay
        Invoke(nameof(ClearResult), 1f);

        timeManager.ResetTime();
    }

    private void ClearResult()
    {
        resultText.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
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
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1; 
        }
    }

}
