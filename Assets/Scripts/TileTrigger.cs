using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileTrigger : MonoBehaviour
{    
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        if (levelManager == null)
        {
            levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }   
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the colliding object is the player
        if (collider.CompareTag("Player"))
        {
            Debug.Log("goal reached");

            WinMenu.LevelIsComplete = true;
            //levelManager.LoadNextScene();
        }
    }
}