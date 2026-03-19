using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{

    // this script is responsible for spawning ghost based on the current tier settings defined in the Game Master
    // it listens for tier changes and updates its spawn settings accordingly
    // it also keeps track of how many ghosts are currently spawned and ensures we don't exceed the maxGhosts limit for the current tier
    // when a ghost is removed (leaves), it updates the count so new ghosts can spawn if we're below the limit


    [Header("References")]
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform spawnPoint;


    [Header("Ghost Pool")]
    [SerializeField] private List<GhostData> availableGhosts; // This list should be populated with all the ghosts that can appear in the game, ideally set in the Inspector
    private int currentGhostCount;
    private int maxGhosts;
    private Vector2 spawnIntervalRange;

    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        gameMaster.OnTierChanged += HandleTierChanged;
    }

    private void OnDisable()
    {
        gameMaster.OnTierChanged -= HandleTierChanged;
    }

    private void HandleTierChanged(TierData tier) // This method is called whenever the tier changes, allowing us to update our spawn settings
    {
        maxGhosts = tier.maxGhosts;
        spawnIntervalRange = tier.spawnIntervalRange;

        RestartSpawning();
    }

    private void RestartSpawning() // Call this whenever the tier changes to reset the spawn loop with new settings
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        if (spawnIntervalRange.x > 0f)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(
                spawnIntervalRange.x,
                spawnIntervalRange.y
            );

            yield return new WaitForSeconds(waitTime);

            if (currentGhostCount < maxGhosts)
            {
                SpawnGhostInternal();
            }
        }
    }

    private void SpawnGhostInternal()
    {

        if (currentGhostCount >= maxGhosts) return;

        GhostData selectedGhost = GetRandomGhost(); // Get a random ghost from the allowed list for the current tier
        if (selectedGhost == null) return;

        GameObject ghostGO = Instantiate( // Spawn the ghost prefab at the designated spawn point
            ghostPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        GhostBehavior ghostBehavior = ghostGO.GetComponent<GhostBehavior>(); // Make sure your ghost prefab has a GhostBehavior component attached
        ghostBehavior.Init(this, gameMaster, selectedGhost); // Pass the selected ghost data to the ghost's behavior script so it can set up its visuals and behavior accordingly



        currentGhostCount++;

    }

    private GhostData GetRandomGhost()
    {
        GhostData[] allowed = gameMaster.CurrentTier.allowedGhosts;
        if (allowed.Length == 0) return null; // safety check

        return allowed[UnityEngine.Random.Range(0, allowed.Length)];
    }

    public void OnGhostRemoved()
    {
        currentGhostCount = Mathf.Max(0, currentGhostCount - 1);
    }

    // DEBUG ONLY
    private void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            SpawnGhostInternal();
        }
    }
}