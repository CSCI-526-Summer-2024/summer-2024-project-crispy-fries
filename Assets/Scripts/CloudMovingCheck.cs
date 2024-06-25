using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovingCheck : MonoBehaviour
{
    public MovingPlatform platform; 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // checkpointManager.PlayerPassedCheckpoint(gameObject.name);
            platform.startMoving();
        }
    }
}