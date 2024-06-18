using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfOGYgVRRp-6DSuW2OLMH2o6p_P-htPqSgNTqotq8QVgaTIzg/formResponse";


    private long randomId;
    private int sceneIndex;
    
    private int[] light;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake() {

        Send();
    }

    public void Send() {
        randomId = Random.Range(100000, 999999);
        sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        light = new int[4];
        light[0] = Random.Range(0, 255);
        light[1] = Random.Range(0, 255);
        light[2] = Random.Range(0, 255);
        light[3] = Random.Range(0, 255);
        StartCoroutine(Post(randomId.ToString(), sceneIndex.ToString(), light));
    }

    private IEnumerator Post(string randomId, string sceneIndex, int[] light)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1662667842", randomId);
        form.AddField("entry.1637880345", sceneIndex);
        form.AddField("entry.1465073703", ((byte)light[0]).ToString());
        form.AddField("entry.1443173421", ((byte)light[1]).ToString());  
        form.AddField("entry.1597942742", ((byte)light[2]).ToString());  
        form.AddField("entry.2028998425", ((byte)light[3]).ToString());  
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


}
