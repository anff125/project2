using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachingManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> enemyList;
    [SerializeField] private Transform door1;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform grass;
    [SerializeField] private Transform grassSpawnPoint;
    private bool isBlockTaught;
    private bool isMoveTaught;
    private bool isMainAttackTaught;
    private bool isSecondaryAttackTaught;
    private bool isSpecialAttackTaught; //water

    private bool doorOpened = false; // Flag to ensure the door only rotates once

    private void Start()
    {
        StartCoroutine(SpawnLavaBall());
    }

    private void Update()
    {
        // If all enemies are null and the door hasn't already opened
        if (!doorOpened && enemyList.TrueForAll(e => e == null))
        {
            StartCoroutine(OpenDoorGradually());
            doorOpened = true; // Set the flag to prevent further rotation
        }
        if (grass == null)
        {
            grass = InstantiateManager.Instance.StartSpawnGrass(grassSpawnPoint.position);
        }
    }
    private IEnumerator OpenDoorGradually()
    {
        float duration = 2f; // Duration in seconds
        float elapsedTime = 0f;
        Quaternion initialRotation = door1.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(Vector3.up * 90f);

        while (elapsedTime < duration)
        {
            door1.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door1.rotation = targetRotation; // Ensure the door reaches the final rotation
    }
    private IEnumerator SpawnLavaBall()
    {
        while (true)
        {
            InstantiateManager.Instance.SpawnLavaBall();
            yield return new WaitForSeconds(5f);
        }
    }

    public void respawnPlayer()
    {
        player.position = respawnPoint.position;
    }
}