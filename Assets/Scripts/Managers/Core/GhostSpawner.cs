
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;
public class GhostSpawner : MonoBehaviour
{

    public GameObject ghostPrefab;
    public Transform spawnPoint;

    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            SpawnGhost();
        }
    }
    public void SpawnGhost()
    {
        Debug.Log("Spawning Ghost...");
        GameObject ghost = Instantiate(ghostPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
