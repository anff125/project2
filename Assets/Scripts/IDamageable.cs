using System;

public interface IDamageable
{
    void TakeDamage(float damage, ElementType elementType = ElementType.Physical);

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
}

// Enum representing possible damage elements
public enum ElementType
{
    Fire,
    Ice,
    Electric,
    Physical
}