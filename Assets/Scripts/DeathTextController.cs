using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class DeathTextController : MonoBehaviour
{
    public TextMeshProUGUI youDiedText; // Reference to the TextMeshProUGUI object
    public float fadeDuration = 5f; // Duration for the fade effect

    void Start()
    {
        if (youDiedText == null)
        {
            youDiedText = GetComponent<TextMeshProUGUI>();
        }
        youDiedText.alpha = 0f;
    }

    public void ShowAndFadeOutText()
    {
        youDiedText.alpha = 1f;
    }

   
}

