using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // How many enemies per second to spawn per 1000 units
    public float spawnRate;
    public Enemy enemyToSpawn;
    public bool debug;

    private bool active;
    private float minimumDistanceFromPlayer;

    public void StartSpawning()
    {
        active = true;
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    public void StopSpawning()
    {
        active = false;
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        yield return null;
        while (active)
        {
            Vector2 extents = CameraManager.EdgeExtents;
            Vector2 spawnLocation = new Vector2(Random.Range(-extents.x + .5f, extents.x -.5f), Random.Range(-extents.y + .5f, extents.y -.5f));
            PlayerController player = FindAnyObjectByType<PlayerController>();
            Vector2 playerLocation = player != null ? FindAnyObjectByType<PlayerController>().transform.position : Vector2.zero;
            minimumDistanceFromPlayer = extents.x * 2 / 4; // Spawn at least 1/4 of the arena from the player

            if (debug) Debug.Log("Attempting enemy spawn...");
            int spawnAttempts = 0;
            while ((Vector2.Distance(spawnLocation,playerLocation) < minimumDistanceFromPlayer) && spawnAttempts < 10)
            {
                spawnLocation = new Vector2(Random.Range(-extents.x, extents.x), Random.Range(-extents.y, extents.y));
                spawnAttempts++;
            }
            if (spawnAttempts < 10) Instantiate(enemyToSpawn, spawnLocation, Quaternion.identity);

            float boundsArea = extents.x *2 * extents.y * 2;
            float timeUntilSpawn = 1000 / (boundsArea * spawnRate);
            if (debug) Debug.Log("Waiting " + timeUntilSpawn + " for next enemy spawn...");
            yield return new WaitForSeconds(timeUntilSpawn);
        }
    }
}
