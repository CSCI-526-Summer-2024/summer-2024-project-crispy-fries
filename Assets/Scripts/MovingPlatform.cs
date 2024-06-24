using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;
    [SerializeField]
    Rigidbody2D rb;

    private void Start()
    {
        targetPos = posB.position;
    }
    void FixedUpdate()
    {

        if(Vector2.Distance(rb.position, posA.position) < 0.1f)
        {
            targetPos = posB.position;
        }
        if (Vector2.Distance(rb.position, posB.position) < 0.1f)
        {
            targetPos = posA.position;
        }
        rb.velocity = speed* (Vector2)(targetPos - transform.position).normalized;
        // rb.MovePosition( (Vector2)(targetPos - transform.position).normalized * speed * Time.deltaTime + rb.position);

    }

    
    
}
