using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class InstantiateManager : MonoBehaviour
{
    [SerializeField] public Transform waterPrefab;
    [SerializeField] public Transform grassPrefab;
    [SerializeField] public Transform aSetOfLavaBallPrefab;
    [SerializeField] public List<Transform> lavaBallSpawnPoints;
    [SerializeField] private Transform turret;
    [SerializeField] public float fieldBoundaryX;
    [SerializeField] public float fieldBoundaryY;

    public static InstantiateManager Instance { get; private set; }
    private readonly float lavaStageDuration = 20f;

    private readonly List<GameObject> activeLavaBalls = new List<GameObject>();
    private readonly int maxLavaBallCount = 20;

    private readonly List<GameObject> activeGrass = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //StartLavaBallStage();
    }

    public void InstantiateWater(Transform spawnPoint)
    {
        Instantiate(waterPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    public void InstantiateWater(Vector3 spawnPoint)
    {
        Instantiate(waterPrefab, spawnPoint, Quaternion.identity);
    }

    public Transform StartSpawnGrass(Vector3 spawnPoint)
    {
        return Instantiate(grassPrefab, spawnPoint, Quaternion.identity);
    }


    public void StartLavaBallStage()
    {
        StartCoroutine(InstantiateLavaBall());
    }



    private IEnumerator InstantiateLavaBall()
    {
        float elapsedTime = 0f;
        GameManager.Instance.mainCamera.gameObject.SetActive(false);
        while (elapsedTime < lavaStageDuration - 6)
        {
            if (elapsedTime >= lavaStageDuration || activeLavaBalls.Count >= maxLavaBallCount)
                break;

            List<Transform> selectedSpawnPoints = GetRandomSpawnPoints(2);
            foreach (var spawnPoint in selectedSpawnPoints)
            {
                GameObject newLavaBall = Instantiate(aSetOfLavaBallPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
                activeLavaBalls.Add(newLavaBall);
            }

            float randomCooldown = Random.Range(3f, 4f);
            yield return new WaitForSeconds(randomCooldown);

            elapsedTime += randomCooldown;
        }
        GameManager.Instance.mainCamera.gameObject.SetActive(true);
        //StartSpawnGrass();
        //turret.gameObject.SetActive(true);
    }
    public void SpawnLavaBall()
    {
        //spawn from lavaBallSpawnPoints[0]
        GameObject newLavaBall = Instantiate(aSetOfLavaBallPrefab, lavaBallSpawnPoints[0].position, lavaBallSpawnPoints[0].rotation).gameObject;
        activeLavaBalls.Add(newLavaBall);

    }

    private List<Transform> GetRandomSpawnPoints(int count)
    {
        List<Transform> selectedPoints = new List<Transform>();
        List<Transform> spawnPointsCopy = new List<Transform>(lavaBallSpawnPoints);

        for (int i = 0; i < count; i++)
        {
            if (spawnPointsCopy.Count == 0) break;
            int randomIndex = Random.Range(0, spawnPointsCopy.Count);
            selectedPoints.Add(spawnPointsCopy[randomIndex]);
            spawnPointsCopy.RemoveAt(randomIndex);
        }

        return selectedPoints;
    }

    public void RemoveDestroyedLavaBall(GameObject lavaBall)
    {
        if (activeLavaBalls.Contains(lavaBall))
        {
            activeLavaBalls.Remove(lavaBall);
        }
    }

    public float GetLavaStageDuration()
    {
        return lavaStageDuration;
    }
}