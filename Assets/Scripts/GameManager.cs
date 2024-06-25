using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpotLightManager spotLightManager;
    public TextUIManager textUIManager;
    public LevelManager levelManager;

    public string buildName = "Post Alpha Prototype Thursday";

    public GameObject player;
    
    private bool gameIsPaused;

    public bool GameIsPaused
    {
        get { return gameIsPaused; }
        set {
                gameIsPaused = value; 
                if(value == true)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
