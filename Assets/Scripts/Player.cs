using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public SpotLightManager spotLightManager;
    bool isInShadow = false;
    private GameObject[] spotLightArray;

    private bool[] lightEnabledArray; 

    public LayerMask obstacleLayers;

    // Start is called before the first frame update
    void Start()
    {
        spotLightArray = spotLightManager.getSpotLightArray();
        lightEnabledArray = spotLightManager.getlightEnabledArray();
    }

    
    void FixedUpdate()
    {
        Move();
        
    }
    // Update is called once per frame
    void Update() {
        spotLightArray = spotLightManager.getSpotLightArray();
        lightEnabledArray = spotLightManager.getlightEnabledArray();
        SetLights();
        for (int i = 0; i < spotLightArray.Length; i++)
        {
            if (lightEnabledArray[i])
            {
                DetectInShadow(spotLightArray[i].transform);
            }
        }
    }

    void Move()
    {
        float moveSpeed = 10;
        //Define the speed at which the object moves.

        float horizontalInput = Input.GetAxis ("Horizontal"); 
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);
        //Move the object to XYZ coordinates defined as horizontalInput, 0, and verticalInput respectively.
    }

    void SetLights(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            spotLightManager.TurnOffLight(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            spotLightManager.TurnOffLight(1);
        }
    }


    void DetectInShadow(Transform lightSource)
    {

        
        Vector3 direction = lightSource.position - transform.position;

        // Check for an obstacle between the object and the light source
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstacleLayers);

        // Debug.DrawRay(transform.position, direction, Color.red);


        if (hit.collider != null)
        {
            Debug.Log("Obstacle detected: " + hit.collider.name + " for " + lightSource.name);
            isInShadow = true;
            
            // spriteRenderer.color = Color.black;
        }
        else
        {
            // No obstacle detected
            Debug.Log("Obstacle not detected for " + lightSource.name);
            isInShadow = false;
            

            FindObjectOfType<LevelManager>().Restart();
            // spriteRenderer.color = Color.white;
        }
    }

}

