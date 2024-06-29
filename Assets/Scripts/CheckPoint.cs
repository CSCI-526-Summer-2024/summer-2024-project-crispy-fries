using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager checkpointManager;
    public string checkpointName;
    public string hintValue;
    public bool hasPassed;
    private TextUIManager textUIManager;
    
    void Start()
    {
        checkpointName = this.name;
        hasPassed = false;
        checkpointManager = FindObjectOfType<CheckpointManager>();
        checkpointManager.RegisterCheckpoint(this.gameObject);
        textUIManager = FindObjectOfType<TextUIManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPassed)
        {
            hasPassed = true;
            checkpointManager.PassCheckpoint(this.gameObject);
            // textUIManager.TriggerHint(hintValue);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // textUIManager.DeTriggerHint();
        }
    }
}
