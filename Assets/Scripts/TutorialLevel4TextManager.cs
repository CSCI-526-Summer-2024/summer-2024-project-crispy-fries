using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel4TextManager : MonoBehaviour
{
 public TextUIManager textUIManager;
    public bool checkSPushed = false;
    
    void Start()
    {
        checkSPushed = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S) && !checkSPushed)
        {
            checkSPushed = true;
            textUIManager.TriggerText1();
        }

    }
}
