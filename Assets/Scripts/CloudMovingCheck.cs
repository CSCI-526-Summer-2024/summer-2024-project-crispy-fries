using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovingCheck : MonoBehaviour
{
    public MovingPlatform platform; 

    void Start()
    {
        platform.startMoving();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            platform.startMoving();
        }
    }
}
