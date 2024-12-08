using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCheck : MonoBehaviour
{
    [SerializeField] private Level2Manager level2Manager;
    private bool _check = false;

    //if player enters the trigger, close all doors in level 2
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_check)
        {
            level2Manager.CloseAllDoors();
            level2Manager.ActivateRoom2Enemies();
            _check = true;
        }
    }


}