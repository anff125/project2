using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public Animator animator;
    public static List<Enemy> ActiveEnemies = new List<Enemy>();
    public bool IsAlliedWithPlayer { get; private set; }
    [SerializeField] public Transform exclamationMark;
    [SerializeField] public Transform bulletPrefab;
    [SerializeField] public float speed = 5f;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int maxFrozenAmount = 50;
    [SerializeField] public int frozenTime = 3;
    protected int WeaponIndex = 0;
    [SerializeField] public float moveDistance;
    [SerializeField] public LayerMask collisionLayerMask;
    [SerializeField] public float attackRange = 10f;
    public float currentHealth;
    public float currentFrozenAmount;

    private bool _isFrozen;
    public bool IsFrozen => _isFrozen;
    private Coroutine _freezeCoroutine;
    private Coroutine _dieCoroutine;
    public event Action<GameObject> OnEnemyDestroyed;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;

    protected virtual float CalculateDamage(IDamageable.Damage damage)
    {
        return damage.Amount;
    }

    public virtual void TakeDamage(IDamageable.Damage damage)
    {
        ElementType elementType = damage.ElementType;

        // If the enemy is frozen and takes fire damage, unfreeze and reset frozen amount
        // Check if health drops to zero or below
        if (currentHealth <= 0)
        {
            if (_dieCoroutine == null)
            {
                _dieCoroutine = StartCoroutine(Die());
            }
            return;
        }
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
        currentHealth -= CalculateDamage(damage);
        OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthNormalized = (float)currentHealth / maxHealth
        });

    }

    private void TakeFrozenProgress(float frozenProgress)
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
        ActiveEnemies.Add(this);
    }
    protected virtual void Start() { }
    protected virtual void Update()
    {
        if (_isFrozen) return;
        EnemyStateMachine.CurrentEnemyState.Update();
    }


    public IEnumerator Die()
    {
        yield return null;
        OnEnemyDestroyed?.Invoke(gameObject);
        ActiveEnemies.Remove(this);
        _isFrozen = true;
        Debug.Log("Enemy destroyed");
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(1f);
        }
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

    public IEnumerator MoveToPosition(Vector3 targetPosition, float time = 1f)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = gameObject.transform.position;
        while (elapsedTime < time)
        {

            // Rotate to face the player
            Vector3 direction = (Player.Instance.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 5f);

            bool canMove = !Physics.BoxCast(transform.position, Vector3.one,
                (targetPosition - startPosition), Quaternion.identity, moveDistance, collisionLayerMask);
            if (canMove)
            {
                gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public Vector3 GetFixedDistancePositionAroundPlayer(float range = 10f)
    {
        float angle = Random.Range(0f, 360f);
        Vector3 randomPosition = Player.Instance.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * range;
        return randomPosition;
    }
    public void SetIsAlliedWithPlayer(bool IsAllied)
    {
        IsAlliedWithPlayer = IsAllied;
    }
}