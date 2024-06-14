using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public TextUIManager textUIManager;
    private Scene currScene;
    private int currBuildIndex;

    void Start()
    {
        currScene = SceneManager.GetActiveScene();
        currBuildIndex = currScene.buildIndex;
        //textUIManager = FindObjectOfType<TextUIManager>();
    }

    public bool pauseInput=false;
    public void Restart() {
        pauseInput=true;
        textUIManager.ShowAndFadeDeath();
    }

    public void LoadNextScene()
    {
        Debug.Log(currBuildIndex);
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (currBuildIndex >= SceneManager.sceneCountInBuildSettings - 1) {
            // At last scene, you win!
            textUIManager.WinGame();
            pauseInput=true;
        }
        else {
            SceneManager.LoadSceneAsync(++currBuildIndex);
        }
    }
}