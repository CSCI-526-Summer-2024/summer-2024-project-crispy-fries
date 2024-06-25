using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public static bool LevelIsComplete = false;

    public GameObject WinMenuUI;

    private string sceneName;
    private int currBuildIndex;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TMP_Text winText;

    public static bool LevelIsPaused = false;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;

    // Start is called before the first frame update
    void Start()
    {
        WinMenuUI.SetActive(false);

        Scene currentScene = SceneManager.GetActiveScene();
        currBuildIndex = currentScene.buildIndex;

        sceneName = currentScene.name;
        levelManager = FindAnyObjectByType<LevelManager>();

        playButton.SetActive(false);
        pauseButton.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (LevelIsComplete)
        {
            WinMenuUI.SetActive(true);
            if (currBuildIndex == SceneManager.sceneCountInBuildSettings - 1) {
                // Last level
                Debug.Log("Last Level!");
                continueButton.SetActive(false);
                winText.text = "Game Completed! Congrats";
            }
            else {
                winText.text = "Level Completed!";
            }
        }
        
    }

    public void Resume()
    {
        levelManager.LoadNextScene();
        LevelIsComplete = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Level Selection");
        LevelIsComplete = false;
        LevelIsPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
        LevelIsComplete = false;
        LevelIsPaused = false;
    }
    public void Pause()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);

        LevelIsPaused = true;
        Time.timeScale = 0f;

        WinMenuUI.SetActive(true);

        continueButton.SetActive(false);

        winText.text = "Pause";
    }
    public void Play()
    {
        playButton.SetActive(false);
        pauseButton.SetActive(true);

        LevelIsPaused = false;
        Time.timeScale = 1f;

        WinMenuUI.SetActive(false);
    }
}
