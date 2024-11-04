using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public Transform exclamationMark;
    [SerializeField] public Transform bulletPrefab;
    [SerializeField] public float speed = 5f;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int maxFrozenAmount = 50;
    [SerializeField] public int frozenTime = 3;
    protected int WeaponIndex = 0;

    public float currentHealth;
    public float currentFrozenAmount;

    private bool _isFrozen;
    private Coroutine _freezeCoroutine;

    public event Action<GameObject> OnEnemyDestroyed;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
    public virtual void TakeDamage(float damage, ElementType elementType = ElementType.Physical)
    {
        // If the enemy is frozen and takes fire damage, unfreeze and reset frozen amount
        if (_isFrozen && elementType == ElementType.Fire)
        {
            if (_freezeCoroutine != null) StopCoroutine(_freezeCoroutine);

            _isFrozen = false;
            currentFrozenAmount = 0;
            OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
            {
                frozenProgressNormalized = 0f
            });
            InstantiateManager.Instance.InstantiateWater(transform);
        }

        // Apply frozen progress if the damage element is Ice
        if (elementType == ElementType.Ice)
        {
            TakeFrozenProgress(1f);
        }

        // Reduce health by the damage amount
        currentHealth -= damage;
        OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthNormalized = (float)currentHealth / maxHealth
        });

        // Check if health drops to zero or below
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void TakeFrozenProgress(float frozenProgress)
    {
        if (currentFrozenAmount >= maxFrozenAmount) return;
        currentFrozenAmount += frozenProgress;
        OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
        {
            frozenProgressNormalized = (float)currentFrozenAmount / maxFrozenAmount
        });
        if (currentFrozenAmount >= maxFrozenAmount)
        {
            _isFrozen = true;
            _freezeCoroutine = StartCoroutine(Unfreeze());
        }
    }

    private IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(frozenTime);
        _isFrozen = false;
        currentFrozenAmount = 0;
        OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
        {
            frozenProgressNormalized = (float)currentFrozenAmount / maxFrozenAmount
        });
    }

    #region States

    protected EnemyStateMachine EnemyStateMachine { get; set; }

    #endregion
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentFrozenAmount = 0;
        _isFrozen = false;
    }
    protected virtual void Start() { }
    protected virtual void Update()
    {
        if (_isFrozen) return;
        EnemyStateMachine.CurrentEnemyState.Update();
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
            Debug.Log("Should not happen in project2. Please check the code");
            //StartCoroutine(Die());
        }
    }

}