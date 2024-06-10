using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI deathText;
    public void Restart() {
        deathText.GetComponent<DeathTextController>().ShowAndFadeOutText();
        // reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
