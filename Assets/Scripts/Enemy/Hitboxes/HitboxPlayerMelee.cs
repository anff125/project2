using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPlayerMelee : MonoBehaviour
{
    [SerializeField] private int damageAmount = 25; // Damage dealt by the weapon
    [SerializeField] private LayerMask layerMask;

    private bool isActive = false; // Tracks if the hitbox is currently active

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.gameObject.layer != gameObject.layer)
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                // enemy.TakeDamage(damageAmount);
                Debug.Log("Hit enemy with weapon hitbox, dealt " + damageAmount + " damage.");
            }
        }
    }

    // Activate the hitbox
    public void ActivateHitbox()
    {
        isActive = true;
        gameObject.SetActive(true); // Enable the hitbox GameObject
    }

    // Deactivate the hitbox
    public void DeactivateHitbox()
    {
        isActive = false;
        gameObject.SetActive(false); // Disable the hitbox GameObject
    }
}
