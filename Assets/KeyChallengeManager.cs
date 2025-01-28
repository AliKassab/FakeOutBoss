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
        return keys[Random.Range(0, keys.Length)].ToString(); // Randomly generate a key
    }

    private Vector2 GenerateRandomPosition()
    {
        RectTransform canvasRect = canvasTransform.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Get the size of the letter UI element
        Vector2 letterSize = keyPopupPrefab.GetComponent<RectTransform>().sizeDelta;

        // Calculate safe bounds
        float xMin = -canvasWidth / 2 + letterSize.x / 2;
        float xMax = canvasWidth / 2 - letterSize.x / 2;
        float yMin = -canvasHeight / 2 + letterSize.y / 2;
        float yMax = canvasHeight / 2 - letterSize.y / 2;

        // Generate random position within safe bounds
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

        currentKeyPopup.GetComponent<RectTransform>().anchoredPosition = position;
    }

    private void DestroyOldKeyPopup()
    {
        if (currentKeyPopup != null)
        {
            Destroy(currentKeyPopup); // Destroy the previous key popup
            currentKeyPopup = null; // Reset the reference
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

        // Reset the time scale back to normal
        timeManager.ResetTime();
    }

    private void ClearResult()
    {
        resultText.gameObject.SetActive(false);
    }
}
