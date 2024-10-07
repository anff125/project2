using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
    event EventHandler<OnHealthChangedEventArgs> OnHealthChange;

    public class OnHealthChangedEventArgs
    {
        public float healthNormalized;
    }
}