using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool hasLifeTime = true;
    private void Start()
    {
        if (hasLifeTime)
            Destroy(gameObject, 5f);
    }

    public void TakeDamage(IDamageable.Damage damage)
    {
        Debug.Log("IceBlock TakeDamage");
        ElementType elementType = damage.ElementType;
        if (elementType == ElementType.Fire)
        {
            TakeMeltingProgress(1f);
        }
    }

    private void TakeMeltingProgress(float frozenProgress)
    {
        if (currentHealth >= maxHealth)
        {
            InstantiateManager.Instance.InstantiateWater(transform);
            Destroy(gameObject);
        }

        currentHealth += frozenProgress;
        OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
        {
            frozenProgressNormalized = (float)currentHealth / maxHealth
        });
    }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
}