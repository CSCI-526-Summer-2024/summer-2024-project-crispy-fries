using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public static bool LevelIsComplete = false;

    public GameObject WinMenuUI;

    private string sceneName;
    private int levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        WinMenuUI.SetActive(false);

        Scene currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;

        // Extract the level number
        levelNumber = GetLevelNumber(sceneName);
    }


    // Update is called once per frame
    void Update()
    {
        if (LevelIsComplete)
        {
            WinMenuUI.SetActive(true);
        }

    }

    int GetLevelNumber(string sceneName)
    {
        // Assuming the scene name format is "level X" where X is the level number
        string[] parts = sceneName.Split(' ');
        if (parts.Length > 1)
        {
            int.TryParse(parts[1], out int levelNumber);
            return levelNumber;
        }
        return -1;
    }

    public void Resume()
    {
        string nextLevelName = "Level " + (levelNumber + 1);

        if (IsLevelInBuildSettings(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            // If the next level does not exist, load the Level Selection scene
            SceneManager.LoadScene("Level Selection");
        }
        LevelIsComplete = false;
    }
    private bool IsLevelInBuildSettings(string levelName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneName == levelName)
            {
                return true;
            }
        }
        return false;
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
