using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public static bool LevelIsComplete = false;

    public GameObject WinMenuUI;


    // Start is called before the first frame update
    void Start()
    {
        WinMenuUI.SetActive(false);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (LevelIsComplete)
        {
            WinMenuUI.SetActive(true);
        }

    }

    public void Resume()
    {
        
        LevelIsComplete = false;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Level Selection");
        LevelIsComplete = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LevelIsComplete = false;
    }
}
