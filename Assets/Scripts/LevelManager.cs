using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI deathText;

    public bool pauseInput=false;
    public void Restart() {
        StartCoroutine(RestartWithDelay());

    }

    private IEnumerator RestartWithDelay()
    {
        pauseInput=true;
        deathText.GetComponent<DeathTextController>().ShowAndFadeOutText();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
