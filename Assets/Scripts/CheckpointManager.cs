using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] allCP;

    private Checkpoint activeCP;
    public Vector3 respawnPosition;
    void Start()
    {
        allCP = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

        foreach (Checkpoint cp in allCP)
        {
            cp.cpMan = this;
        }

        respawnPosition = FindFirstObjectByType<PlayerController>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivataeAllCheckpoints()
    {
        foreach (Checkpoint cp in allCP)
        {
            cp.DeactivateCheckpoint();
        }
    }

    public void SetActiveCheckpoint(Checkpoint newActiveCP)
    {
        DeactivataeAllCheckpoints();
        activeCP = newActiveCP;
        respawnPosition = newActiveCP.transform.position;
    }
}
