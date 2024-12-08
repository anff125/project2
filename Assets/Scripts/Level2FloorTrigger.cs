using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2FloorTrigger : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    public List<GameObject> enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers the event
        {
            foreach (var enemy in enemies)
            {
                enemy.SetActive(true);
            }
            gameObject.SetActive(false); // Disable the trigger so it only activates once
        }
    }
}
