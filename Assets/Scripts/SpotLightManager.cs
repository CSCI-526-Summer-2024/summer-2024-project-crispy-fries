using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpotLightManager : MonoBehaviour
{

    private GameObject[] spotLightArray;

    private List<GameObject> toggleableLights = new List<GameObject>();
    private int toggleableLightSelectedIndex = 0;

    [SerializeField]
    private GameObject selector;
    private Vector3 targetPosition;

    private GameManager gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spotLightArray = FindObjectsOfType<SpotLightController>().Select(controller => controller.gameObject).ToArray();

        foreach (GameObject spotlight in spotLightArray)
        {
            SpotLightController controller = spotlight.GetComponent<SpotLightController>();
            if (controller != null && controller.isToggleable)
            {
                toggleableLights.Add(spotlight);
            }
        }
        
        toggleableLights = toggleableLights.OrderBy(light => light.transform.position.x).ToList();
        if (toggleableLights.Count == 0) 
        {
            selector.SetActive(false);
            toggleableLightSelectedIndex=-1;
        }
        setTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!gameManager.GameIsPaused  && Input.GetKeyDown(KeyCode.A) && toggleableLightSelectedIndex!=-1)
        {
            toggleableLightSelectedIndex++;
            if (toggleableLightSelectedIndex >= toggleableLights.Count)
                toggleableLightSelectedIndex = 0;
            setTargetPosition();
        }

        if (!gameManager.GameIsPaused  && Input.GetKeyDown(KeyCode.D))
        {
            ToggleSelectedLight();
        }
        selector.transform.position = Vector3.Lerp(selector.transform.position, targetPosition, 10 * Time.deltaTime);
        selector.transform.Rotate(Vector3.forward, 100 * Time.deltaTime);

        // Rotate Spotlights
        foreach (GameObject spotlight in spotLightArray){
            if (spotlight.GetComponent<SpotLightController>().doesRotate){
                RotateSpotlight(spotlight, 0.5f);
            }
        }
        // r, 160, 230
        // g, 0, 90
        // g(1), 270, 180
        // b 0, 270
        // y, 150, 210
    }

    void ToggleSelectedLight()
    {
        if (toggleableLightSelectedIndex!=-1)
        {
            TurnOffLight(toggleableLightSelectedIndex);
        }
    }

    void setTargetPosition()
    {
        if (toggleableLightSelectedIndex!=-1)
        {
            targetPosition = toggleableLights[toggleableLightSelectedIndex].transform.position;
            
        }
    }

    /*
     *  Turns off a single specified light.
     *  All other lights will be turned on.
     *  Ensure Toggleable lights come before non toggleable ones
     */
    public void TurnOffLight(int lightNum){
        // Switch Specified Light Off
        toggleableLights[lightNum].GetComponent<SpotLightController>().toggleLight();

        // Ensure all other lights are on
        for (int i = 0; i < toggleableLights.Count; i++){
            if (i != lightNum){
                toggleableLights[i].GetComponent<SpotLightController>().toggleLightOn();
            }
        }
    }

    public GameObject[] getSpotLightArray(){
        return spotLightArray;
    }

    public void RotateSpotlight(GameObject spotlight, float freq)
    {
        SpotLightController controller = spotlight.GetComponent<SpotLightController>();
        Quaternion from = Quaternion.Euler(new Vector3(0,0,controller.rotationAngleFrom));
        Quaternion to = Quaternion.Euler(new Vector3(0,0,controller.rotationAngleTo));
        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * freq));
        spotlight.transform.localRotation = Quaternion.Lerp(from, to, lerp);
    }

    
}
