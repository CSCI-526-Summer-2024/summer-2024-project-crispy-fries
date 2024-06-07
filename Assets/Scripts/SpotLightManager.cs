using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightManager : MonoBehaviour
{
    public GameObject[] spotLightArray;
    private bool[] lightEnabledArray;
    
    // Start is called before the first frame update
    void Start()
    {
        lightEnabledArray = new bool[spotLightArray.Length];
        // Initialize all lights to be on
        for (int i = 0; i < spotLightArray.Length; i++){
            lightEnabledArray[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     *  Turns off a single specified light in the lightEnabledArray.
     *  All other lights will be left on.
     *  As of now, we dont really need lightEnabledArray but it may be handy in 
     *  the future when implementing the ability to turn multiple lights off.
     */
    public void TurnOffLight(int lightNum){
        // Switch Specified Light Off
        spotLightArray[lightNum].SetActive(false);
        lightEnabledArray[lightNum] = false;

        // Ensure all other lights are on
        for (int i = 0; i < lightEnabledArray.Length; i++){
            if (i != lightNum){
                lightEnabledArray[i] = true;
                spotLightArray[i].SetActive(true);
            }
        }
    }
}
