using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public bool LevelIsComplete = false;

    public GameObject WinMenuUI;

    private string sceneName;
    private int currBuildIndex;

    private LevelManager levelManager;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private TMP_Text winText;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;

    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject controlsGroup;

    [SerializeField] private GameObject pausedMenuGroup;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Scene currentScene = SceneManager.GetActiveScene();
        currBuildIndex = currentScene.buildIndex;

        sceneName = currentScene.name;
        levelManager = FindAnyObjectByType<LevelManager>();

        pauseButton.SetActive(true);
        playButton.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        if(!LevelIsComplete && Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameManager.GameIsPaused) this.Play();
            else this.Pause();
        }
        
    }

    public void WinGame()
    {
        LevelIsComplete = true;
        pausedMenuGroup.SetActive(true);
        resumeButton.SetActive(false);
        pauseButton.SetActive(false);

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

    public void NextLevel()
    {
        levelManager.LoadNextScene();
        LevelIsComplete = false;
        gameManager.GameIsPaused = false;

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
        pausedMenuGroup.SetActive(true);
        nextLevelButton.SetActive(false);
        pauseButton.SetActive(false);
        playButton.SetActive(true);
        gameManager.GameIsPaused = true;

        winText.text = "Paused";
       
    }
    public void Play()
    {
        controlsGroup.SetActive(false);
        pausedMenuGroup.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        gameManager.GameIsPaused = false;
        
    }
}
