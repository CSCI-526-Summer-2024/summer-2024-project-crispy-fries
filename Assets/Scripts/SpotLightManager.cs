using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] spotLightArray;

    [SerializeField]
    private int toggleableLightCount;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            TurnOffLight(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            TurnOffLight(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            TurnOffLight(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)){
            TurnOffLight(3);
        }

    }

    /*
     *  Turns off a single specified light.
     *  All other lights will be turned on.
     *  Ensure Toggleable lights come before non toggleable ones
     */
    public void TurnOffLight(int lightNum){
        if(lightNum>= toggleableLightCount) return;
        // Switch Specified Light Off
        spotLightArray[lightNum].GetComponent<SpotLightController>().toggleLight();

        // Ensure all other lights are on
        for (int i = 0; i < toggleableLightCount; i++){
            if (i != lightNum){
                spotLightArray[i].GetComponent<SpotLightController>().toggleLightOn();
            }
        }
    }

    public GameObject[] getSpotLightArray(){
        return spotLightArray;
    }

    
}
