using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    private Dictionary<string, bool> checkpoints = new Dictionary<string, bool>();

    public void RegisterCheckpoint(string checkpointName)
    {
        if (!checkpoints.ContainsKey(checkpointName))
        {
            checkpoints.Add(checkpointName, false); 
        }
    }

    public void PlayerPassedCheckpoint(string checkpointName)
    {
        if (checkpoints.ContainsKey(checkpointName))
        {
            checkpoints[checkpointName] = true;
            Debug.Log("Player passed through checkpoint: " + checkpointName);
        }
    }

    public List<string> GetPassedCheckpoints()
    {
        List<string> passedCheckpoints = new List<string>();

        foreach (var checkpoint in checkpoints)
        {
            if (checkpoint.Value)
            {
                passedCheckpoints.Add(checkpoint.Key);
            }
        }

        return passedCheckpoints;
    }

    public bool IsCheckpointPassed(string checkpointName)
    {
        return checkpoints.ContainsKey(checkpointName) && checkpoints[checkpointName];
    }

    public List<string> GetAllCheckpoints()
    {
        List<string> allCheckpoints = new List<string>(checkpoints.Keys);
        return allCheckpoints;
    }
}
