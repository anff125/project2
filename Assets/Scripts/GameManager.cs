using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Create a singleton
    public static GameManager Instance { get; private set; }

    // Spawn Enemy every 5 seconds
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> bossEnemyPrefabs;
    [SerializeField] private int maxEnemiesInScene; // Maximum number of enemies allowed
    [SerializeField] private float spawnWindow; // Maximum number of enemies allowed

    [SerializeField] private float wallMinX = -36f;
    [SerializeField] private float wallMaxX = 36f;
    [SerializeField] private float wallMinZ = -18f;
    [SerializeField] private float wallMaxZ = 18f;
    // [SerializeField] private Text waveText;  // 用於顯示波次提示的UI文字元件
    private int currentWave = 1;  // 當前波數


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
        StartCoroutine(SpawnEnemy());
        // Uncomment to spawn boss enemy after 10 seconds
        //StartCoroutine(SpawnBossEnemy());
    }

    private IEnumerator SpawnBossEnemy()
    {
        yield return new WaitForSeconds(5);
        Instantiate(bossEnemyPrefabs[0], new Vector3(0, 0, 20), Quaternion.Euler(0, 0, 0));
    }

    private Vector3 GetSpawnPositionWithinWalls(Vector3 spawnPosition)
    {
        float clampedX = Mathf.Clamp(spawnPosition.x, wallMinX, wallMaxX);
        float clampedZ = Mathf.Clamp(spawnPosition.z, wallMinZ, wallMaxZ);
        
        // Y 不做限制，假設高度不變
        return new Vector3(clampedX, spawnPosition.y, clampedZ);
    }

    // private void UpdateWaveText()
    // {
    //     // 更新 UI 顯示文字
    //     waveText.text = $"{currentWave} / 4";
    // }
    
    private IEnumerator SpawnEnemy()
    {
        // 定義每波敵人的相對位置（以玩家為中心）
        List<Vector3> relativePositions = new List<Vector3>
        {
            new Vector3(Mathf.Cos(60f * Mathf.Deg2Rad), 0, Mathf.Sin(60f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(30f * Mathf.Deg2Rad), 0, Mathf.Sin(30f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(0f), 0, Mathf.Sin(0f)),
            new Vector3(Mathf.Cos(-30f * Mathf.Deg2Rad), 0, Mathf.Sin(-30f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(-60f * Mathf.Deg2Rad), 0, Mathf.Sin(-60f * Mathf.Deg2Rad))
        };

        // 手動選擇每波敵人的種類
        List<GameObject> selectedEnemyPrefabs = new List<GameObject>
        {
            enemyPrefabs[0], // Shotgun
            enemyPrefabs[1], // Rifle
            enemyPrefabs[2] // Fire
            // enemyPrefabs[3] // Radar
        };

        for (int i = 0; i < 4; i++)
        {
            // 更新波數提示
            currentWave = i + 1;
            // UpdateWaveText();  // 更新UI顯示

            if (i == 0) yield return new WaitForSeconds(5);
            else        yield return new WaitForSeconds(spawnWindow);
            
            // 獲取玩家當前位置
            Vector3 playerPosition = Player.Instance.transform.position;

            // 為每個相對位置生成敵人
            for (int j = 0; j < relativePositions.Count; j++)
            {
                Vector3 spawnPosition = playerPosition + relativePositions[j] * 15; // 計算敵人的生成位置
                // 確保敵人生成在牆內
                spawnPosition = GetSpawnPositionWithinWalls(spawnPosition);

                // 生成指定種類的敵人 and add it to the list
                if (i == 0) // Round 1, Shotguns only
                {
                    GameObject newEnemy = Instantiate(selectedEnemyPrefabs[0], spawnPosition, Quaternion.identity);
                    spawnedEnemies.Add(newEnemy);
                    // Ensure the enemy gets removed from the list when destroyed
                    newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += RemoveEnemyFromList;
                }
                else if (i == 1)
                {
                    GameObject newEnemy;
                    // 2 Shotguns and 3 Rifles
                    if (j == 0 || j == 4)
                    {
                        newEnemy = Instantiate(selectedEnemyPrefabs[0], spawnPosition, Quaternion.identity);
                    }
                    else
                    {
                        newEnemy = Instantiate(selectedEnemyPrefabs[1], spawnPosition, Quaternion.identity);
                    }
                    spawnedEnemies.Add(newEnemy);
                    // Ensure the enemy gets removed from the list when destroyed
                    newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += RemoveEnemyFromList;
                }
                else if (i == 2)
                {
                    GameObject newEnemy;
                    // 2 Shotguns and 3 Fires
                    if (j == 0 || j == 4)
                    {
                        newEnemy = Instantiate(selectedEnemyPrefabs[0], spawnPosition, Quaternion.identity);
                    }
                    else
                    {
                        newEnemy = Instantiate(selectedEnemyPrefabs[2], spawnPosition, Quaternion.identity);
                    }
                    spawnedEnemies.Add(newEnemy);
                    // Ensure the enemy gets removed from the list when destroyed
                    newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += RemoveEnemyFromList;
                }
                else if (i == 3)
                {
                    GameObject newEnemy = Instantiate(selectedEnemyPrefabs[2], spawnPosition, Quaternion.identity);
                    spawnedEnemies.Add(newEnemy);
                    // Ensure the enemy gets removed from the list when destroyed
                    newEnemy.GetComponent<Enemy>().OnEnemyDestroyed += RemoveEnemyFromList;
                }
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