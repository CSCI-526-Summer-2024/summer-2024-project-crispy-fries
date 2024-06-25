using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTextManager : MonoBehaviour
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
        if ((Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E)) && !checkQEPushed && !checkShiftPushed)
        {
            checkQEPushed = true;
            textUIManager.TriggerText1();
        }

        if (checkQEPushed && Input.GetKeyUp(KeyCode.LeftShift) && !checkShiftPushed) {
            textUIManager.TriggerText2();
            checkShiftPushed = true;
        }

    }
}
