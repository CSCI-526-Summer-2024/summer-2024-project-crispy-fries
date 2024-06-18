using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{

    [SerializeField] private string sceneName;  
    public TMP_Text levelText;
    // Start is called before the first frame update
    void Start()
    {
        levelText.text = sceneName;
    }

    public void OpenScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
