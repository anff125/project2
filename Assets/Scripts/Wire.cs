using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour, IDamageable
{
    [SerializeField] private LayerMask layerMask;
    public void TakeDamage(IDamageable.Damage damage)
    {
        if (damage.ElementType == ElementType.Electric)
        {
            Debug.Log("Wire is damaged");
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation, layerMask);
            foreach (var colliderOut in colliders)
            {
                Debug.Log("Collided with " + colliderOut.name);
                if (colliderOut.CompareTag("PowderKeg"))
                {
                    IDamageable damageable = colliderOut.GetComponent<IDamageable>();
                    damageable.TakeDamage(damage);
                }
                else if (colliderOut.gameObject.layer == LayerMask.NameToLayer("Turret"))
                {
                    IDamageable damageable = colliderOut.GetComponent<IDamageable>();
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
    
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;

}