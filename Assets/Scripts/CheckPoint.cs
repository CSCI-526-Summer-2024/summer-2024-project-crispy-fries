using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager checkpointManager;

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
        }
    }
}
