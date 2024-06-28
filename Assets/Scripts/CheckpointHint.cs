using Unity.VisualScripting;
using UnityEngine;

public class CheckpointHint : MonoBehaviour
{
    private CheckpointHintManager checkpointHintManager;
    [SerializeField] private string checkpointName;
    [SerializeField] private Vector3 offset;
    private GameObject player;
    public string hintValue;
    public bool hasPassed;
    private TextUIManager textUIManager;
    
    void Start()
    {
        hasPassed = false;
        checkpointHintManager = FindObjectOfType<CheckpointHintManager>();
        checkpointHintManager.RegisterCheckpoint(this.gameObject);
        textUIManager = FindObjectOfType<TextUIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {   
        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint (player.transform.position + offset); // pass the world position
        textUIManager.hintText.transform.position = screenPosition; // set the UI Transform's position as it will accordingly adjust the RectTransform values
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPassed)
        {
            hasPassed = true;
            checkpointHintManager.PassCheckpoint(this.gameObject);
            textUIManager.TriggerHint(hintValue);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textUIManager.DeTriggerHint();
        }
    }
}
