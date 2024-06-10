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
            if(spotLightArray[i].GetComponent<SpotLightController>().DoesIlluminate(gameObject.transform.position, obstacleLayers))
            {
                FindObjectOfType<LevelManager>().Restart();
            }
        }
    }

    void Move()
    {
        float horizontalMoveSpeed = 10;
        float verticalMoveSpeed = 7;


        float horizontalInput = Input.GetAxis ("Horizontal"); 
        //Get the value of the Horizontal input axis.

        float verticalInput = Input.GetAxis("Vertical");
        //Get the value of the Vertical input axis.
        
        transform.Translate(new Vector3(horizontalInput*horizontalMoveSpeed, verticalInput*verticalMoveSpeed, 0) * Time.deltaTime);
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


    

}

