using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public Transform player;
    public Vector3 respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the player to the respawn point
            other.transform.position = respawnPoint;
        }
    }
}
