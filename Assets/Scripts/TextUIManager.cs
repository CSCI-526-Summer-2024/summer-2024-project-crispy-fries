using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TextUIManager : MonoBehaviour
{
    // References to ALL UI Text Elements
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private TextMeshProUGUI winText;
    public TextMeshProUGUI hintText;
    Vector3 hintOffset = Vector3.zero;

    private Coroutine fadeCoroutine;
    private float fadeDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI hintText2;

    [SerializeField] private TextMeshProUGUI triggeredText;

    [SerializeField] private TextMeshProUGUI triggeredText1;
    [SerializeField] private TextMeshProUGUI triggeredText2;
    [SerializeField] private GameObject player;
    [SerializeField] private GridLayout grid;
    private bool secondHint;

    private Scene currScene;

    // Start is called before the first frame update
    void Start()
    {
        ShowAndFadeLevel();
        ShowAndFadeHint();
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        secondHint = false;
    }
    void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint (player.transform.position + hintOffset); // pass the world position
        hintText.transform.position = screenPosition; // set the UI Transform's position as it will accordingly adjust the RectTransform values
        // ShowSecondHint();
    }

    public void ShowAndFadeLevel()
    {
        StartCoroutine(ShowAndFadeLevelCoroutine());
    }
    
    public void ShowAndFadeDeath(){
        StartCoroutine(ShowAndFadeDeathCoroutine());
    }

    public void ShowAndFadeHint(){
        //StartCoroutine(ShowAndFadeHintCoroutine());
    }

    public void TriggerHint(CheckpointHint hint){
        hintOffset = hint.offset;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInText(hint.hintValue));
    }

    public void DeTriggerHint(){
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeInText(string text)
    {
        hintText.text = text;
        hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, 0);
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, 1);
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0;
        Color originalColor = hintText.color;

        while (elapsedTime < fadeDuration)
        {
            hintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - (elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        hintText.text = "";
    }

    public void TriggerText(){
        StartCoroutine(ShowTriggeredText());
    }

    public void TriggerText1(){
        StartCoroutine(ShowTriggeredText1());
    }

    public void TriggerText2(){
        StartCoroutine(ShowTriggeredText2());
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
        if (hintText2 != null)
        hintText2.alpha = 0;
        if (triggeredText != null) {
            triggeredText.alpha = 0;
        }
        if (triggeredText1 != null) {
            triggeredText1.alpha = 0;
        }
        if (triggeredText2 != null) {
            triggeredText2.alpha = 0;
        }
        winText.alpha = 0;
        levelText.alpha = 1f;
        yield return new WaitForSeconds(5);
        levelText.alpha = 0f;
        winText.alpha = 0;
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

    private IEnumerator WinGameCoroutine()
    {
        winText.alpha = 1f;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    private IEnumerator ShowTriggeredText()
    {
        if (triggeredText == null) {
            yield break;
        }
        triggeredText.alpha = 1f;
        yield return new WaitForSeconds(10);
        triggeredText.alpha = 0;
    }


    private IEnumerator ShowTriggeredText1()
    {
        if (triggeredText1 == null) {
            yield break;
        }
        hintText.alpha = 0;
        triggeredText1.alpha = 1f;
        yield return new WaitForSeconds(10);
        triggeredText1.alpha = 0;
    }
    private IEnumerator ShowTriggeredText2()
    {
        if (triggeredText2 == null) {
            yield break;
        }
        triggeredText1.alpha = 0;
        triggeredText2.alpha = 1f;
        yield return new WaitForSeconds(10);
        triggeredText2.alpha = 0;
    }

    public void ShowSecondHint(){
        if (hintText2 == null){
            return;
        }
        Vector3 playerPos = grid.WorldToCell(player.transform.position);
        //Debug.Log(playerPos);

        if (playerPos == new Vector3(-8.0f,-3.0f,0.0f)){
            StartCoroutine(ShowSecondHintCoroutine());
        }
    }
    private IEnumerator ShowSecondHintCoroutine(){
        if (secondHint == true){
            yield break;
        }
        Debug.Log("show second hint");
        hintText.alpha = 0;
        hintText2.alpha = 1;
        yield return new WaitForSeconds(8);
        hintText2.alpha = 0;
        secondHint = true;
    }
}
