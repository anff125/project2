using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachingManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    private bool isBlockTaught;
    private bool isMoveTaught;
    private bool isMainAttackTaught;
    private bool isSecondaryAttackTaught;
    private bool isSpecialAttackTaught; //water

    private void Start()
    {
        StartCoroutine(SpawnLavaBall());
    }

    private IEnumerator SpawnLavaBall()
    {
        while (true)
        {
            InstantiateManager.Instance.SpawnLavaBall();
            yield return new WaitForSeconds(5f);
        }
    }
    
    
}