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
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
