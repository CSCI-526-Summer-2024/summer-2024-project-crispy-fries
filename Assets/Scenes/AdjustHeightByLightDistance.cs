using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeightByLightDistance : MonoBehaviour
{
    public Transform lightSource;
    public float heightMultiplier = 1.0f;
    public LayerMask obstacleLayers;

    void Update()
    {
        AdjustHeight();
    }

    void AdjustHeight()
{
    RaycastHit hit;
    Vector3 direction = lightSource.position - transform.position;

    // Check for an obstacle between the object and the light source
    if (Physics.Raycast(transform.position, direction, out hit, direction.magnitude, obstacleLayers))
    {
        Debug.Log("Obstacle detected: " + hit.collider.name);
        // Optionally, handle the situation when there is an obstacle
    }
    else
    {
        // No obstacle detected, adjust the height based on the distance to the light source
        // float distance = direction.magnitude;
        // float newHeight = distance * heightMultiplier;
        // transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
        Debug.Log("Obstacle not detected");
    }
}

}
