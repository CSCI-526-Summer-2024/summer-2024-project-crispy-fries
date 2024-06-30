using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    private List<GameObject> checkpoints = new();
    private TextUIManager textUIManager;

    void Start()
    {
        textUIManager = FindObjectOfType<TextUIManager>();
    }

    public void RegisterCheckpoint(GameObject checkpoint)
    {
        checkpoints.Add(checkpoint);
    }

    public void PassCheckpoint(GameObject checkpoint)
    {
        textUIManager.TriggerText();
    }

    public List<string> GetPassedCheckpoints()
    {
        List<string> passedCheckpoints = new List<string>();

        foreach (GameObject checkpoint in checkpoints)
        {
            bool hasPassed = checkpoint.GetComponent<Checkpoint>().hasPassed;
            string checkpointName = checkpoint.GetComponent<Checkpoint>().checkpointName;
            if (hasPassed)
            {
                passedCheckpoints.Add(checkpointName);
            }
        }
        return passedCheckpoints;
    }
}
