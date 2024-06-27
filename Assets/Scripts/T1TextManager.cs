using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1TextManager : MonoBehaviour
{
    public TextUIManager textUIManager;
    public bool checkShiftPushed = false;
    public bool checkQEPushed = false;
    void Start()
    {
        checkQEPushed = false;
        checkShiftPushed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S) && !checkQEPushed && !checkShiftPushed)
        {
            checkQEPushed = true;
            textUIManager.TriggerText1();
        }

        else if (checkQEPushed && (Input.GetKeyUp(KeyCode.S) ||Input.GetKeyUp(KeyCode.W)) && !checkShiftPushed) {
            textUIManager.TriggerText2();
            checkShiftPushed = true;
        }

    }
}
