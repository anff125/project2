using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private TeachingManager teachingManager;
    //if player falls into the dead zone, respawn the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            teachingManager.respawnPlayer();
        }
    }
}