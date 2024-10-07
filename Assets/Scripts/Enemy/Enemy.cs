using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public Transform bulletPrefab;
    protected int WeaponIndex = 0;
    public float currentHealth;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    #region States

    protected EnemyStateMachine EnemyStateMachine { get; set; }

    #endregion
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }
    protected virtual void Start() { }
    protected virtual void Update()
    {
        EnemyStateMachine.CurrentEnemyState.Update();
    }
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthNormalized = (float)currentHealth / maxHealth
        });
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());

        }
    }
    
    private IEnumerator Die()
    {
        yield return null;
        Player.Instance.WearWeapon(WeaponIndex);
        Destroy(gameObject);
    }

}