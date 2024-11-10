using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private int maxHealth;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float skillCooldown;
    [SerializeField] private float shieldCooldown;
    [SerializeField] private float dashCooldown;

    [SerializeField] private SkillUI skillCooldownUI;
    [SerializeField] private SkillUI dashCooldownUI;

    private Camera _camera;

    private float skillTimer;
    private bool skillOnCooldown;

    private float shieldTimer;
    private bool shieldOnCooldown;

    private bool dashOnCooldown;
    private float dashTimer;

    private float _currentHealth;
    public static Player Instance { get; private set; }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
    public void TakeDamage(IDamageable.Damage damage)
    {
        _currentHealth -= damage.Amount;
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
    }

    private void Start()
    {
        _camera = Camera.main;
        GameInput.Instance.OnMainAttackStarted += OnMainAttackStarted;
        GameInput.Instance.OnMainAttack += OnMainAttack;
        GameInput.Instance.OnMainAttackCancelled += OnMainAttackCancelled;

        GameInput.Instance.OnSecondaryAttackStarted += OnSecondaryAttackStarted;
        GameInput.Instance.OnSecondaryAttack += OnSecondaryAttack;
        GameInput.Instance.OnSecondaryAttackCancelled += OnSecondaryAttackCancelled;

        GameInput.Instance.OnMainSkillStart += OnMainSkillStart;
        GameInput.Instance.OnMainSkillCancelled += OnMainSkillCancelled;

        GameInput.Instance.OnShield += OnShield;
        GameInput.Instance.OnDash += OnDash;

        shield.GetComponent<Shield>().OnBlockSuccess += BlockSuccess;

        mainAttackParameters.effect.Stop();
        secondaryAttackParameters.effect.Stop();
    }

    private void Update()
    {
        HandleMovement();

        if (dashOnCooldown)
        {
            dashTimer -= Time.deltaTime;

            dashCooldownUI.coolDownMaskImage.fillAmount = dashTimer / dashCooldown;
            var color = dashCooldownUI.mainSkillImage.color;
            color.a = 125 / 255f; // Alpha value should be between 0 and 1
            dashCooldownUI.mainSkillImage.color = color;

            if (dashTimer <= 0)
            {
                dashOnCooldown = false;
                color.a = 1; // Alpha value should be between 0 and 1
                dashCooldownUI.mainSkillImage.color = color;
            }
        }

        if (skillOnCooldown)
        {
            skillTimer -= Time.deltaTime;

            skillCooldownUI.coolDownMaskImage.fillAmount = skillTimer / skillCooldown;
            var color = skillCooldownUI.mainSkillImage.color;
            color.a = 125 / 255f; // Alpha value should be between 0 and 1
            skillCooldownUI.mainSkillImage.color = color;

            if (skillTimer <= 0)
            {
                skillOnCooldown = false;
                color.a = 1; // Alpha value should be between 0 and 1
                skillCooldownUI.mainSkillImage.color = color;
            }
        }

        if (shieldOnCooldown)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldOnCooldown = false;
            }
        }
    }

    private void OnMainAttack(object sender, EventArgs e) { }
    private void OnSecondaryAttack(object sender, EventArgs e) { }

    private void OnMainAttackStarted(object sender, EventArgs e) { StartAttacking(mainAttackParameters); }
    private void OnMainAttackCancelled(object sender, EventArgs e) { StopAttacking(mainAttackParameters); }

    private void OnSecondaryAttackStarted(object sender, EventArgs e) { StartAttacking(secondaryAttackParameters); }
    private void OnSecondaryAttackCancelled(object sender, EventArgs e) { StopAttacking(secondaryAttackParameters); }

    #region Shield

    [SerializeField] Transform shield;
    [SerializeField] private float shieldDuration;
    private Coroutine shieldCoroutine;

    private void BlockSuccess(object sender, EventArgs e)
    {
        if (shieldOnCooldown)
        {
            shieldTimer = 0;
            shieldOnCooldown = false;
        }
    }

    private void OnShield(object sender, EventArgs e)
    {
        if (shieldOnCooldown) return;

        shieldOnCooldown = true;
        shieldTimer = shieldCooldown;
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldCoroutine());

        //push object around away


    }
    private IEnumerator ShieldCoroutine()
    {
        shield.gameObject.SetActive(true);
        yield return new WaitForSeconds(shieldDuration);
        shield.gameObject.SetActive(false);
    }

    #endregion

    #region Skill

    [SerializeField] private GameObject lightingVisualSupportPrefab; // Assign in the inspector
    [SerializeField] private float skillRange;
    [SerializeField] private AttackParameters skillAttackParameters;

    private Coroutine skillCoroutine;
    private GameObject skillVisualSupport;
    private void OnMainSkillStart(object sender, EventArgs e)
    {
        if (skillOnCooldown)
        {
            Debug.Log("Skill is on cooldown.");
            return;
        }
        skillOnCooldown = true;
        skillTimer = skillCooldown;

        if (skillCoroutine == null)
        {
            skillVisualSupport = Instantiate(lightingVisualSupportPrefab);
            //set skillAttackParameters.range to skillVisualSupport x scale
            skillAttackParameters.range = skillVisualSupport.transform.localScale.x / 2;
            skillCoroutine = StartCoroutine(SkillCoroutine());
        }
    }

    private void OnMainSkillCancelled(object sender, EventArgs e)
    {
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
            //deal damage to enemies
            DealDamage(skillVisualSupport.transform, skillAttackParameters);
            Destroy(skillVisualSupport);
        }
    }

    private IEnumerator SkillCoroutine()
    {
        while (true)
        {
            Vector3 mousePosition = GetCursorPointOnGround();

            Vector3 directionToCursor = (mousePosition - transform.position).normalized;
            float distanceToCursor = Vector3.Distance(transform.position, mousePosition);
            if (distanceToCursor > skillRange)
            {
                mousePosition = transform.position + directionToCursor * skillRange;
            }

            skillVisualSupport.transform.position = mousePosition + Vector3.up * 0.1f;
            yield return null;
        }
    }

  #endregion

    #region Attack

    private bool isAttacking;

    private Coroutine mainAttackCoroutine;

    [Serializable]
    public struct AttackParameters
    {
        public float range;
        public float angle;
        public float damage;
        public ElementType elementType;
        public ParticleSystem effect;
    }

    [SerializeField] private AttackParameters mainAttackParameters;
    [SerializeField] private AttackParameters secondaryAttackParameters;

    private AttackParameters lastParameters;
    private void PerformAttack(AttackParameters parameters)
    {
        DealDamage(transform, parameters);
    }

    private void AdjustAttackEffect(AttackParameters parameters)
    {
        if (parameters.effect == null)
        {
            Debug.LogError("Effect is null. Please assign it in the inspector.");
            return;
        }

        var shapeModule = parameters.effect.shape;
        shapeModule.enabled = true;
        shapeModule.shapeType = ParticleSystemShapeType.Cone;
        shapeModule.angle = parameters.angle * 0.5f;
        shapeModule.radius = 0f;

        var mainModule = parameters.effect.main;
        float startSpeed = mainModule.startSpeed.constant;
        if (startSpeed == 0)
        {
            Debug.LogWarning("StartSpeed is zero. Setting it to a default value of 5.");
            startSpeed = 5f;
            mainModule.startSpeed = startSpeed;
        }

        mainModule.startLifetime = parameters.range / startSpeed;
    }

    private Coroutine currentAttackCoroutine;

    private IEnumerator AttackCoroutine(AttackParameters parameters)
    {
        while (true)
        {
            PerformAttack(parameters);
            yield return null;
        }
    }

    private void StartAttacking(AttackParameters parameters)
    {
        if (currentAttackCoroutine == null)
        {
            lastParameters = parameters;
            AdjustAttackEffect(parameters);
            parameters.effect.Play();
            currentAttackCoroutine = StartCoroutine(AttackCoroutine(parameters));
        }
        else
        {
            StopAttacking(lastParameters);
            StartAttacking(parameters);
        }
    }

    private void StopAttacking(AttackParameters parameters)
    {
        if (currentAttackCoroutine != null && lastParameters.effect == parameters.effect)
        {
            StopCoroutine(currentAttackCoroutine);
            parameters.effect.Stop();
            currentAttackCoroutine = null;
        }
    }

    #endregion

    #region Movement

    private void OnDash(object sender, EventArgs e)
    {
        if (!dashOnCooldown)
        {
            float dashDistance = 5f;
            Vector2 move = GameInput.Instance.GetMovementVector();
            float playerRadius = .7f;
            Vector3 moveDir = new Vector3(move.x, 0, move.y);

            RaycastHit hitInfo;
            bool canDash = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir,
                out hitInfo, Quaternion.identity, dashDistance, collisionLayerMask);

            Vector3 dashTarget;
            if (canDash)
            {
                // No obstacles within dash range, dash the full distance
                dashTarget = transform.position + moveDir * dashDistance;
            }
            else
            {
                // Obstacle detected, dash to the hit point
                dashTarget = hitInfo.point - moveDir.normalized * playerRadius * 1.5f;
            }
            dashTarget.y = 0;
            transform.position = dashTarget;
            dashOnCooldown = true;
            dashTimer = dashCooldown;
        }
        else
        {
            Debug.Log("Dash is on cooldown.");
        }
    }


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
            // Check if we can move forward
            Vector3 forwardDir = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.BoxCast(transform.position,
                Vector3.one * playerRadius,
                forwardDir, Quaternion.identity, moveDistance, collisionLayerMask);

            // Check if we can move to the side
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
                    // Can't move forward or to the side
                    moveDir = Vector3.zero;
                }
            }
        }

        transform.position += (moveDir * moveDistance);

        // Rotate towards the cursor position
        Vector3 cursorPosition = GetCursorPointOnGround();
        if (cursorPosition != Vector3.zero) // Ensure we have a valid point
        {
            Vector3 lookDirection = (cursorPosition - transform.position).normalized;
            lookDirection.y = 0; // Keep the rotation horizontal
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, lookDirection, Time.deltaTime * rotateSpeed);
        }
    }

  #endregion

    private void Die()
    {
        Debug.Log("Player has died");
    }

    private void DealDamage(Transform attacker, AttackParameters parameters)
    {
        Collider[] hits = Physics.OverlapSphere(attacker.position, parameters.range, ~LayerMask.GetMask("Player"));
        foreach (Collider hit in hits)
        {
            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                // 使用 ClosestPoint 來取得碰撞體表面最接近攻擊者的點
                Vector3 closestPoint = hit.ClosestPoint(attacker.position);
                closestPoint.y = 0;
                Vector3 directionToTarget = (closestPoint - attacker.position).normalized;
                float angleToTarget = Vector3.Angle(attacker.forward, directionToTarget);

                if (angleToTarget <= parameters.angle)
                {
                    IDamageable.Damage damage = new IDamageable.Damage(parameters.damage, parameters.elementType, attacker);
                    target.TakeDamage(damage);
                }
            }
        }
    }

    private Vector3 GetCursorPointOnGround()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        //return point of ray hit layer ground
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
        {
            Vector3 mousePosition = hit.point;
            return mousePosition;
        }
        return Vector3.zero;
    }
}