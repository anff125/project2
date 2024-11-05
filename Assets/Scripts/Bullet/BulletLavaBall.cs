using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletLavaBall : Bullet, IDamageable
{
    [SerializeField] public int maxFrozenAmount = 50;
    public float currentFrozenAmount;


    public void TakeDamage(float damage, ElementType elementType = ElementType.Physical)
    {
        // Apply frozen progress if the damage element is Ice
        if (elementType == ElementType.Ice)
        {
            TakeFrozenProgress(1f);
        }
    }

    private void TakeFrozenProgress(float frozenProgress)
    {
        if (currentFrozenAmount >= maxFrozenAmount) Destroy(gameObject);
        currentFrozenAmount += frozenProgress;
        OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
        {
            frozenProgressNormalized = (float)currentFrozenAmount / maxFrozenAmount
        });

    }
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
}