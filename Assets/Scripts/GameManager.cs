using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    

    public SpotLightManager spotLightManager;
    public TextUIManager textUIManager;
    public LevelManager levelManager;

    public CheckpointManager checkpointManager;

    public string buildName = "Beta Build";

    public GameObject player;
    
    private bool gameIsPaused;

    public  LevelStars levelStars;

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
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
        Application.targetFrameRate = 60;
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void CollectStar(int starIndex)
    {
        if(starIndex == 1)
        {
            levelStars.star1 = true;
        }
        else if(starIndex == 2)
        {
            levelStars.star2 = true;
        }
        else if(starIndex == 3)
        {
            levelStars.star3 = true;
        }
        FindObjectOfType<WinMenu>().SetStars(levelStars);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinGame()
    {
        StarManager.instance.CollectStar(SceneManager.GetActiveScene().name,levelStars);
        player.GetComponent<PlayerController>().Win();
        FindObjectOfType<WinMenu>().WinGame();
        gameIsPaused = true;
    }
}
