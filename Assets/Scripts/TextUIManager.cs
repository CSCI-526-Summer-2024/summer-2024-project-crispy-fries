using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextUIManager : MonoBehaviour
{
    // References to ALL UI Text Elements
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI hintText;

    private Scene currScene;

    // Start is called before the first frame update
    void Start()
    {
        ShowAndFadeLevel();
        ShowAndFadeHint();
    }

    public void ShowAndFadeLevel()
    {
        StartCoroutine(ShowAndFadeLevelCoroutine());
    }
    
    public void ShowAndFadeDeath(){
        StartCoroutine(ShowAndFadeDeathCoroutine());
    }

    public void ShowAndFadeHint(){
        StartCoroutine(ShowAndFadeHintCoroutine());
    }

    public void WinGame(){
        StartCoroutine(WinGameCoroutine());
    }

    // Only to be called by ShowAndFadeLevelName()
    private IEnumerator ShowAndFadeLevelCoroutine()
    {
         // Get active screen
        currScene = SceneManager.GetActiveScene();
        string sceneName = currScene.name;
        levelText.text = sceneName;
        deathText.alpha = 0f;
        hintText.alpha = 0;
        winText.alpha = 0;
        levelText.alpha = 1f;
        yield return new WaitForSeconds(2);
        levelText.alpha = 0f;
    }

    // Only to be called by ShowAndFadeDeath()
    private IEnumerator ShowAndFadeDeathCoroutine()
    {
        deathText.text = "You Died";
        deathText.alpha = 1f;
        levelText.alpha = 0f;
        yield return new WaitForSeconds(1);
        deathText.alpha = 0f;

        // LoadScene must be called in coroutine because
        // LoadScene runs asyncronously and will mess up WaitForSeconds()
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ShowAndFadeHintCoroutine()
    {   
        hintText.alpha = 1;
        yield return new WaitForSeconds(2);
        hintText.alpha = 0;
    }

    private IEnumerator WinGameCoroutine()
    {
        winText.alpha = 1f;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
