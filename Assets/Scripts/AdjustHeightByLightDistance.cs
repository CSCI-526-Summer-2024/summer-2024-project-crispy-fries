using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeightByLightDistance : MonoBehaviour
{
    public Transform lightSource;
    public LayerMask obstacleLayers;

    public float normalScale = 1.0f;
    public float shrinkScale = 0.1f;

    void Update()
    {
        AdjustHeight();
    }

    void AdjustHeight()
    {
        Vector3 direction = lightSource.position - transform.position;

        // Check for an obstacle between the object and the light source
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstacleLayers);

        Debug.DrawRay(transform.position, direction, Color.red);


        if (hit.collider != null)
        {
            Debug.Log("Obstacle detected: " + hit.collider.name);
            transform.localScale = new Vector3(transform.localScale.x, shrinkScale, transform.localScale.z);
        }
        else
        {
            // No obstacle detected
            transform.localScale = new Vector3(transform.localScale.x, normalScale, transform.localScale.z);
            Debug.Log("Obstacle not detected");
        }
    }

}
