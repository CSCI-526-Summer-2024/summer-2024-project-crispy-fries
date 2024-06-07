using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public SpotLightManager spotLightManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    
    void FixedUpdate()
    {
        Move();
        
    }
    // Update is called once per frame
    void Update() {
        SetLights();
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

}

