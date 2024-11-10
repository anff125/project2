using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(Damage damage);

    event EventHandler<OnHealthChangedEventArgs> OnHealthChange;
    event EventHandler<OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;

    public class OnHealthChangedEventArgs
    {
        public float healthNormalized;
    }

    public class OnFrozenProgressChangedEventArgs
    {
        public float frozenProgressNormalized;
    }

    public class Damage
    {
        public readonly float Amount;
        public readonly ElementType ElementType;
        public readonly Transform Source;
        public Damage(float i, ElementType physical, Transform transform)
        {
            Amount = i;
            ElementType = physical;
            Source = transform;
        }
    }

}

// Enum representing possible damage elements
public enum ElementType
{
    Fire,
    Ice,
    Electric,
    Physical
}