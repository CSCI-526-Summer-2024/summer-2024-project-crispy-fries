using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCardController : MonoBehaviour
{
    [SerializeField] private string levelName;

    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;
    public TMP_Text levelText;


    // Start is called before the first frame update
    void Start()
    {
        levelText.text = levelName;
        SetStars();
    }

    public void OpenScene()
    {
        SceneManager.LoadScene(levelName);
    }

    void SetStars()
    {
        LevelStars stars = StarManager.instance.GetLevelStars(levelName);
        Debug.Log(stars.star1);
        star1.SetActive(stars.star1);
        star2.SetActive(stars.star2);
        star3.SetActive(stars.star3);
    }
}
