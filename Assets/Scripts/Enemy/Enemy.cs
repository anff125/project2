using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public Transform exclamationMark;
    [SerializeField] public float speed = 5f;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public Transform bulletPrefab;
    protected int WeaponIndex = 0;
    public float currentHealth;

    public event Action<GameObject> OnEnemyDestroyed;

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

        OnEnemyDestroyed?.Invoke(gameObject);

        Destroy(gameObject);
    }

    public int TotalBulletsShot { get; set; }

    private int _reflectedBullets;
    public int ReflectedBullets
    {
        get => _reflectedBullets;
        set
        {
            _reflectedBullets = value;
            CheckIfDestroyed();
        }
    }

    public void RegisterBullet(Bullet bullet)
    {
        bullet.SetShooter(this);
    }

    public void IncrementReflectedCount()
    {
        ReflectedBullets++;
    }

    private void CheckIfDestroyed()
    {
        if (ReflectedBullets == TotalBulletsShot && TotalBulletsShot > 0)
        {
            StartCoroutine(Die());
        }
    }

}