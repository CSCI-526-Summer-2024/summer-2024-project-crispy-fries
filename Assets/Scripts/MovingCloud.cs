using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    public bool moving;
    Vector3 targetPos;

    private void Start()
    {
        targetPos = posB.position;
        moving = false;
    }
    void Update()
    {
        if (!moving)
        {
            return;
        }
        if(Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            targetPos = posB.position;
        }
        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            targetPos = posA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed* Time.deltaTime);

    }
    public void startMoving() {
        moving = true;
    }
    
}
