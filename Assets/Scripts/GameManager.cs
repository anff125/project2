using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    //Create a singleton
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool isBossFight;
    // Spawn Enemy every 5 seconds
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> bossEnemyPrefabs;
    [SerializeField] private int maxEnemiesInScene; // Maximum number of enemies allowed
    [SerializeField] private float spawnWindow; // Maximum number of enemies allowed
    [SerializeField] public CinemachineVirtualCamera mainCamera;
    [SerializeField] public CinemachineVirtualCamera farCamera;

    private readonly List<GameObject> spawnedEnemies = new List<GameObject>(); // To keep track of spawned enemies

    public void ChangeMaxEnemiesInScene(int maxEnemies)
    {
        maxEnemiesInScene = maxEnemies;
    }

    public void ChangeSpawnWindow(float newSpawnWindow)
    {
        spawnWindow = newSpawnWindow;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!isBossFight)
        {
            StartCoroutine(SpawnEnemy());
        }
        //StartCoroutine(SpawnEnemy());
        // Uncomment to spawn boss enemy after 10 seconds
        //StartCoroutine(SpawnBossEnemy());
    }

    private IEnumerator SpawnBossEnemy()
    {
        yield return new WaitForSeconds(5);
        Instantiate(bossEnemyPrefabs[0], new Vector3(0, 0, 20), Quaternion.Euler(0, 0, 0));
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnWindow);

            // Check if the number of enemies in the scene is less than the max allowed
            if (spawnedEnemies.Count < maxEnemiesInScene)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Count);

                // Calculate a random position around the player in a circle of radius 10
                Vector3 playerPosition = Player.Instance.transform.position;
                float angle = Random.Range(0f, 360f);
                Vector3 spawnPosition = playerPosition + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 10;

                // Spawn enemy and add it to the list
                GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(newEnemy);

                // Ensure the enemy gets removed from the list when destroyed
                newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += RemoveEnemyFromList;
            }
        }
    }

    // Method to remove enemy from the list when destroyed
    private void RemoveEnemyFromList(GameObject enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
        }
    }

}