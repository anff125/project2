using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBoss2Melee : MonoBehaviour
{
    [SerializeField] public float damage = 10;
    [SerializeField] private EnemyBoss2 enemyBoss;
    [SerializeField] private int playerLayer = 7;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log("Boss2 Hit: " + other.name);
            Player player = other.GetComponent<Player>();
            if (player == null) return;

            // if (player.IsParrying == true)
            // {
            //     Debug.Log("    Boss2 been parried by " + other.name);
            //     enemyBoss.beenParried = true;
            // }
            // else
            // {
            //     Debug.Log("    Boss2 damage");
            //     player.TakeDamage(damage);
            // }
        }
    }
}
