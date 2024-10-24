using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Create a singleton
    public static GameManager Instance { get; private set; }

    //Spawn Enemy every 5 seconds
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> bossEnemyPrefabs;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        //spawn boss enemy after 10 seconds
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
            yield return new WaitForSeconds(3);
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
    
            // Calculate a random position around the player in a circle of radius 10
            Vector3 playerPosition = Player.Instance.transform.position;
            float angle = Random.Range(0f, 360f);
            Vector3 spawnPosition = playerPosition + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 10;
    
            Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }
}