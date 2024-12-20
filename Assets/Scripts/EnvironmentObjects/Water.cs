using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;

    private bool isTriggered;
    private void Start()
    {
        Destroy(gameObject, 20f);
    }

    public void TakeDamage(IDamageable.Damage damage)
    {
        if (damage.ElementType == ElementType.Electric)
        {
            if (isTriggered) return;
            isTriggered = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x * 0.5f);
            foreach (var colliderOut in colliders)
            {
                if (colliderOut.CompareTag("Water") || colliderOut.gameObject.layer == LayerMask.NameToLayer("Wire"))
                {
                    IDamageable damageable = colliderOut.GetComponent<IDamageable>();
                    damageable.TakeDamage(damage);
                }

                else if (colliderOut.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    IDamageable damageable = colliderOut.GetComponent<IDamageable>();
                    damageable.TakeDamage(damage);
                }


            }
            Destroy(gameObject);
        }
    }
}