using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroupHandler : MonoBehaviour
{
    [SerializeField] private  GameObject parent;
    // Start is called before the first frame update

    public void OpenGroup()
    {
        gameObject.SetActive(true);
        parent.SetActive(false);
    }

    public void CloseGroup()
    {
        gameObject.SetActive(false);
        parent.SetActive(true);
    }
}
