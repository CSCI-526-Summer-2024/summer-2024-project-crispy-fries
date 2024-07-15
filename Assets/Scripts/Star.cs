using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public int starIndex; // Set this to 1, 2, or 3 for each star in the Inspector
    [SerializeField]

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the star count in StarManager
            LevelStars newStars = new LevelStars(
                starIndex == 1,
                starIndex == 2,
                starIndex == 3
            );

            GameManager.instance.levelStars = GameManager.instance.levelStars.Union(newStars);

            // Destroy the star after collection
            Destroy(gameObject);
        }
    }
}
