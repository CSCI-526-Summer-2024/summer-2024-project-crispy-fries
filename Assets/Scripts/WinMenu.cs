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

    // Start is called before the first frame update
    void Start()
    {
        WinMenuUI.SetActive(false);

        Scene currentScene = SceneManager.GetActiveScene();
        currBuildIndex = currentScene.buildIndex;

        sceneName = currentScene.name;
        levelManager = FindAnyObjectByType<LevelManager>();
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
                winText.text = "You win!";
            }
            else {
                winText.text = "Level Complete!";
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
    }
    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
        LevelIsComplete = false;
    }
}
