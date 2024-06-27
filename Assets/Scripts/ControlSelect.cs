using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSelect : MonoBehaviour
{
    [SerializeField] private  GameObject controlsGroup;
    [SerializeField] private  GameObject menuGroup;
    // Start is called before the first frame update
    void Start()
    {
        controlsGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenControls()
    {
        controlsGroup.SetActive(true);
        menuGroup.SetActive(false);
    }

    public void CloseControls()
    {
        controlsGroup.SetActive(false);
        menuGroup.SetActive(true);
    }
}
