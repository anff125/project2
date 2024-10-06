using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Create a singleton
    public static GameManager Instance { get; private set; }

    //Spawn Enemy every 5 seconds
    [SerializeField] private List<GameObject> enemyPrefabs;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
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