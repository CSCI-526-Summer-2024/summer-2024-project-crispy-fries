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

    private LevelManager levelManager;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private TMP_Text winText;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;

    [SerializeField] private GameObject resumeButton;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        WinMenuUI.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
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
            resumeButton.SetActive(false);
            WinMenuUI.SetActive(true);
            if (currBuildIndex == SceneManager.sceneCountInBuildSettings - 1) {
                // Last level
                Debug.Log("Last Level!");
                nextLevelButton.SetActive(false);
                
                winText.text = "Game Completed! Congrats";
            }
            else {
                nextLevelButton.SetActive(true);
                winText.text = "Level Completed!";
            }
        }
        
    }

    public void NextLevel()
    {
        levelManager.LoadNextScene();
        LevelIsComplete = false;
        gameManager.GameIsPaused = false;

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Level Selection");
        LevelIsComplete = false;
        gameManager.GameIsPaused = false;

    }
    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
        LevelIsComplete = false;
        gameManager.GameIsPaused = false;
    }
    public void Pause()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        nextLevelButton.SetActive(false);
        resumeButton.SetActive(true);

        gameManager.GameIsPaused = true;

        winText.text = "Paused";
        WinMenuUI.SetActive(true);

        

       
    }
    public void Play()
    {
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        gameManager.GameIsPaused = false;
        WinMenuUI.SetActive(false);
    }
}
