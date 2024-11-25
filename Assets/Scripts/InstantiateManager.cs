using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstantiateManager : MonoBehaviour
{
    [SerializeField] public Transform waterPrefab;
    [SerializeField] public Transform aSetOfLavaBallPrefab;
    [SerializeField] public List<Transform> lavaBallSpawnPoints;
    public static InstantiateManager Instance { get; private set; }
    private readonly float lavaStageDuration = 20f;

    private readonly List<GameObject> activeLavaBalls = new List<GameObject>();
    private readonly int maxLavaBallCount = 20;

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

            float randomCooldown = Random.Range(2f, 3f);
            yield return new WaitForSeconds(randomCooldown);

            elapsedTime += randomCooldown;
        }
        GameManager.Instance.mainCamera.gameObject.SetActive(true);
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