using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2FloorTrigger1 : MonoBehaviour
{
    [SerializeField] private Level3Manager level3Manager;
    private bool _check = false;

    //if player enters the trigger, close all doors in level 2
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_check)
        {
            level3Manager.ActivateRoom1Enemies();
            _check = true;
        }
    }
}
