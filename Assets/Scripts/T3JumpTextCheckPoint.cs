using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T3JumpTextCheckPoint : MonoBehaviour
{
public MovingPlatform platform;
    private bool isCoroutineRunning = false; 
    private Coroutine delayedStartCoroutine; 
    public TextMeshProUGUI triggeredText2;
    void Start()
    {
        triggeredText2.alpha = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCoroutineRunning)
        {
            delayedStartCoroutine = StartCoroutine(DelayedStart(3f));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isCoroutineRunning)
        {
            StopCoroutine(delayedStartCoroutine);
            isCoroutineRunning = false;
        }
    }

    IEnumerator DelayedStart(float delay)
    {
        isCoroutineRunning = true; 
        yield return new WaitForSeconds(delay);
        triggeredText2.alpha = 1;
        isCoroutineRunning = false;
    }
}
