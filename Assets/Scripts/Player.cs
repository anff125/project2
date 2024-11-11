using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private int maxHealth = 100;
    //[SerializeField] private int maxMana = 10;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private int startingWeaponIndex;
    [SerializeField] private Transform playerMeleeTransform;
    [SerializeField] public HitboxPlayerMelee weaponHitbox;
    private PlayerMelee playerMelee;
    private Weapon currentWeapon = null;
    private float _currentHealth;
    public static Player Instance { get; private set; }
    // private float _currentMana;
    private bool _isMoving;
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    private float parryTimer;
    private bool isParrying;
    public bool IsParrying => isParrying;
    private bool isAttacking;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _currentHealth = maxHealth;
        isParrying = false;
        parryTimer = 0f;
    }
    private void Start()
    {
        playerMelee = playerMeleeTransform.GetComponent<PlayerMelee>();
        // weaponHolder.Init();
        // WearWeapon(startingWeaponIndex);
        
        // GameInput.Instance.OnMainAttack += OnMainAttack;
        // GameInput.Instance.OnSecondaryAttackStarted += OnSecondaryAttackStarted;
        // GameInput.Instance.OnSecondaryAttackCancelled += OnSecondaryAttackCancelled;
        GameInput.Instance.OnMeleeAttack += OnMeleeAttack;
        GameInput.Instance.OnDash += OnDash;
        GameInput.Instance.OnParry += OnParry;
    }

    private void OnMeleeAttack(object sender, EventArgs e)
    {
        playerMelee.Swing();
    }

    private void OnSecondaryAttackStarted(object sender, EventArgs e) { StartSecondaryAttacking(); }
    private void OnSecondaryAttackCancelled(object sender, EventArgs e) { StopSecondaryAttacking(); }
    

    private void StartSecondaryAttacking()
    {
        isAttacking = true;
        if (currentWeapon == null)
        {
            Debug.LogError("No weapon is equipped to start secondary attack");
        }
        currentWeapon.StartSecondaryAttack();
        StartCoroutine(SecondaryAttackContinuously());
    }

    private void StopSecondaryAttacking()
    {
        isAttacking = false;
        if (currentWeapon == null)
        {
            Debug.LogError("No weapon is equipped to stop secondary attack");
        }
        StopCoroutine(SecondaryAttackContinuously());
        currentWeapon.SecondaryAttack();
        if (currentWeapon.index != 0)
        {
            WearWeapon(0);
        }
    }

    private IEnumerator SecondaryAttackContinuously()
    {
        while (isAttacking)
        {
            currentWeapon.DrawVisualSupport();
            yield return null;
        }
    }

    private void OnDash(object sender, EventArgs e)
    {
        //Dash forward with a 1f cooldown
        if (_isMoving)
        {
            transform.position += transform.forward * 2f;
        }
    }
    private void OnParry(object sender, EventArgs e)
    {
        Debug.Log("Parry!");
        parryTimer = 1 / 6;
        isParrying = true;
    }
    public void WearWeapon(int index)
    {
        if (currentWeapon != null)
        {
            currentWeapon.HideVisualSupport();
            currentWeapon.Hide();
        }
        currentWeapon = weaponHolder.GetWeapon(index);
        Debug.Log("Wearing weapon: " + currentWeapon.name);
        currentWeapon.Show();
        //if secondary attack is being used show visual support
        if (isAttacking)
        {
            currentWeapon.ShowVisualSupport();
        }
    }
    private void OnMainAttack(object sender, EventArgs e)
    {
        if (currentWeapon == null)
        {
            Debug.LogError("No weapon is equipped to main attack");
        }
        currentWeapon.MainAttack();
    }

    private void Update()
    {
        HandleMovement();
        HandleParryTimer();
        //HandleMana();
    }

    // private void HandleMana()
    // {
    //     if (_currentMana >= maxMana) return;
    //     _currentMana += Time.deltaTime;
    // }
    public void Moveto(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
    private void HandleMovement()
    {
        Vector2 move = GameInput.Instance.GetMovementVector();
        Vector3 moveDir = new Vector3(move.x, 0, move.y);

        var moveDistance = (moveSpeed * Time.deltaTime);
        float playerRadius = 0.7f;
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius,
            moveDir, Quaternion.identity, moveDistance, collisionLayerMask);

        if (!canMove)
        {
            //1. check if we can move forward
            Vector3 forwardDir = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.BoxCast(transform.position,
                Vector3.one * playerRadius,
                forwardDir, Quaternion.identity, moveDistance, collisionLayerMask);

            //2. check if we can move to the side
            if (canMove)
            {
                moveDir = forwardDir;
            }
            else
            {
                Vector3 sideDir = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.BoxCast(transform.position,
                    Vector3.one * playerRadius,
                    sideDir, Quaternion.identity, moveDistance, collisionLayerMask);

                if (canMove)
                {
                    moveDir = sideDir;
                }
                else
                {
                    //Can't move forward or to the side
                    moveDir = Vector3.zero;
                }
            }
        }

        _isMoving = moveDir != Vector3.zero;

        transform.position += (moveDir * moveDistance);
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void HandleParryTimer()
    {
        if (isParrying && parryTimer <= 0)
        {
            isParrying = false;
            return;
        }

        parryTimer -= Time.deltaTime;
    }

    // public float GetCurrentMana()
    // {
    //     return _currentMana;
    // }
    //
    // public void UseMana(float amount)
    // {
    //     _currentMana -= amount;
    // }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthNormalized = _currentHealth / maxHealth
        });
        if (_currentHealth <= 0)
        {
            Die();
            _currentHealth = maxHealth;
        }
    }
    private void Die()
    {
        Debug.Log("Player has died");
    }

}