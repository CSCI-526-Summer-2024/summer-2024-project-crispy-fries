using UnityEngine;

public class CheckpointForHint : MonoBehaviour
{
    private CheckpointManager checkpointManager;
    public TextUIManager textUIManager;

    void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
        checkpointManager.RegisterCheckpoint(gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointManager.PlayerPassedCheckpoint(gameObject.name);
            textUIManager.TriggerText();
        }
    }
}
