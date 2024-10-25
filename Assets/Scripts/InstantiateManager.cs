using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateManager : MonoBehaviour
{
    [SerializeField] public Transform waterPrefab;
    public static InstantiateManager Instance { get; private set; }

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

    public void InstantiateWater(Transform transform)
    {
        Instantiate(waterPrefab, transform.position, transform.rotation);
    }
}