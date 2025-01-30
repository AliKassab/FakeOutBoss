using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeyChallengeManager : MonoBehaviour
{
    public GameObject keyPopupPrefab;
    public Transform canvasTransform;
    private bool challengeActive = false;
    private string currentKey = "";

    


    public static event Action<bool, bool> OnAltTab;

    private GameObject currentKeyPopup;

    private Vector2 randomPosition;
    public TimeManager timeManager;
    private int correctPresses = 0;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private GameObject vol;

    [SerializeField] private GameObject e;
    [SerializeField] private GameObject l;
     public GameLogic g;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource audioSource3;
    [SerializeField] private AudioSource audioSource4;
    [SerializeField] private AudioClip keyPressSound;

    [SerializeField] private GameObject Fail;
    //private void Start()
    //{
    //    StartKeyChallenge();
    //}

    private void Update()
    {
        if (challengeActive && Input.anyKeyDown)
        {
            CheckKeyPress();
        }
    }

    public void StartKeyChallenge()
    {
        
        challengeActive = true;
        correctPresses = 0;
        currentKey = GenerateRandomKey();
        randomPosition = GenerateRandomPosition();
        DisplayKeyPopup(currentKey, randomPosition);
        timeManager.DoSlowmotion();
        vol.SetActive(true);
        e.SetActive(true);
        l.SetActive(false);
        audioSource2.pitch = 0.5f;
        audioSource3.pitch = 0.5f;
    }


    
    private void CheckKeyPress()
    {
        if (Input.GetKeyDown(currentKey.ToLower()))
        {
            audioSource.PlayOneShot(keyPressSound);
            currentKey = GenerateRandomKey();
            randomPosition = GenerateRandomPosition();

            DestroyOldKeyPopup();
            DisplayKeyPopup(currentKey, randomPosition);
            correctPresses++;
            if (correctPresses == 3)
            {
                DestroyOldKeyPopup();
                vol.SetActive(false);
                timeManager.ResetTime();
                EndKeyChallenge();
                bossAI.KeyChallengeSuccess();
                e.SetActive(false);
                l.SetActive(true);
                correctPresses = 0;
                audioSource2.pitch = 1f;
                audioSource3.pitch = 1f;
            }

        }
        else
        {
            DestroyOldKeyPopup();
            audioSource2.Pause();
            audioSource3.Pause();
            audioSource4.Pause();
            EndKeyChallenge();
            e.SetActive(false);
            l.SetActive(true);
            vol.SetActive(false);
            audioSource2.pitch = 1f;
            audioSource3.pitch = 1f;
            timeManager.ResetTime();
            bossAI.KeyChallengeFail();
            Fail.SetActive(true);
            Time.timeScale = 0f;




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


    private void EndKeyChallenge()
    {
        challengeActive = false;

        

        // Destroy key pop-ups (clear the canvas)
        //foreach (Transform child in canvasTransform)
        //{
        //    Destroy(child.gameObject);
        //}

        // Reset time after a short delay
        vol.SetActive(false);
        timeManager.ResetTime();

    }

    public bool IsChallengeActive()
    {
        return challengeActive;
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
            elapsedTime += Time.unscaledDeltaTime; // ✅ Use unscaled time
            yield return null;
        }


        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
    }


}
