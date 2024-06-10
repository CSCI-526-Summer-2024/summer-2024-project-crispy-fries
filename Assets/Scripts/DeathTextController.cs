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
        // Start the fade out coroutine
        StartCoroutine(FadeOutText());
        


    }

    private IEnumerator FadeOutText()
    {
        // Ensure the text is fully visible at the start
        youDiedText.alpha = 1f;

        // Wait for a moment before starting the fade out (optional)
        yield return new WaitForSecondsRealtime(2f);
        youDiedText.alpha = 0f;

    }
}

