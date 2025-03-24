using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditNames : MonoBehaviour
{
    [SerializeField] List<string> names;
    private static int index;
    private void OnEnable()
        => GetComponent<TextMeshProUGUI>().text = names[index];

    private void OnDisable()
    {
        if (index >= names.Count-1)
            index = 0;
        else
            index++;
    }

}
