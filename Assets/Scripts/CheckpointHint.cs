using Unity.VisualScripting;
using UnityEngine;

public class CheckpointHint : MonoBehaviour
{
    private CheckpointHintManager checkpointHintManager;
    [SerializeField] private string checkpointName;
    public string hintValue;
    public bool hasPassed;
    private TextUIManager textUIManager;
    
    void Start()
    {
        hasPassed = false;
        checkpointHintManager = FindObjectOfType<CheckpointHintManager>();
        checkpointHintManager.RegisterCheckpoint(this.gameObject);
        textUIManager = FindObjectOfType<TextUIManager>();
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
