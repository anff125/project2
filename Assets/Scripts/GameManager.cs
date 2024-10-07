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
        StartCoroutine(SpawnBossEnemy());
    }
    private IEnumerator SpawnBossEnemy()
    {
        yield return new WaitForSeconds(10);
        Instantiate(bossEnemyPrefabs[0], new Vector3(0, 0, 20), Quaternion.Euler(0, 0, 0));
    }
    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            Instantiate(enemyPrefabs[randomIndex], new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);
        }
    }
}