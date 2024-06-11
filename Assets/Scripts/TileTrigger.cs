using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileTrigger : MonoBehaviour
{
    
    public string nextSceneName; // Name of the next scene to load

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the colliding object is the player
        if (collider.CompareTag("Player"))
        {
            Debug.Log("goal reached");
            SceneManager.LoadScene(nextSceneName);
            
        }
    }

}