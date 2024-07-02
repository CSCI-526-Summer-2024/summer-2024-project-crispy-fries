using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager checkpointManager;
    public string checkpointName;
    public string hintValue;
    public bool hasPassed;
    
    void Start()
    {
        checkpointName = this.name;
        hasPassed = false;
        checkpointManager = FindObjectOfType<GameManager>().checkpointManager;
        checkpointManager.RegisterCheckpoint(this.gameObject);
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
}
