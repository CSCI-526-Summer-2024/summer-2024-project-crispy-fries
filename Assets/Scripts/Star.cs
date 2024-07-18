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
            // LevelStars newStars = new LevelStars(
            //     starIndex == 1,
            //     starIndex == 2,
            //     starIndex == 3
            // );

            GameManager.instance.CollectStar(starIndex);

            // Destroy the star after collection
            Destroy(gameObject);
        }
    }
    void Update()
    {
        // Calculate the new scale based on a sine wave
        float scaleX = Mathf.Sin(Time.time*3);
        transform.localScale = new Vector3(scaleX, 1, 1);
    }
}
