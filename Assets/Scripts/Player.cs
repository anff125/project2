using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private int maxHealth;

    [SerializeField] private float maxMoveSpeed;
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

    public event EventHandler OnDeath;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;

    [SerializeField] private bool canBeInvincible;
    private bool isInvincible = false;
    private float invincibilityDuration = 1.5f;
    private Coroutine invincibilityCoroutine;
    [SerializeField] private Transform playerVisual;
    public void TakeDamage(IDamageable.Damage damage)
    {
        if (isInvincible) return; // Ignore damage if already invincible

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
        else if (canBeInvincible)
        {
            // Trigger invincibility after taking damage
            if (invincibilityCoroutine != null) StopCoroutine(invincibilityCoroutine);
            invincibilityCoroutine = StartCoroutine(HandleInvincibility());
        }
    }
    private IEnumerator HandleInvincibility()
    {
        isInvincible = true;

        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < invincibilityDuration)
        {
            // Toggle between normal and transparent material
            if (!isVisible)
            {
                playerVisual.gameObject.SetActive(false);
            }
            else
            {
                playerVisual.gameObject.SetActive(true);
            }
            isVisible = !isVisible;

            // Wait for a short interval before toggling again
            yield return new WaitForSeconds(0.2f);

            elapsed += 0.2f;
        }

        // Restore to normal material and disable invincibility
        playerVisual.gameObject.SetActive(true);
        isInvincible = false;
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
        createWaterParameters.effect.Stop();
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

    private void OnMainAttackStarted(object sender, EventArgs e)
    {
        buttonState = buttonState == AttackButtonState.Right ? AttackButtonState.Both : AttackButtonState.Left;
        StartAttacking();
    }
    private void OnMainAttackCancelled(object sender, EventArgs e)
    {
        buttonState = buttonState == AttackButtonState.Both ? AttackButtonState.Right : AttackButtonState.None;
        StopAttacking();
    }

    private void OnSecondaryAttackStarted(object sender, EventArgs e)
    {
        buttonState = buttonState == AttackButtonState.Left ? AttackButtonState.Both : AttackButtonState.Right;
        StartAttacking();
    }
    private void OnSecondaryAttackCancelled(object sender, EventArgs e)
    {
        buttonState = buttonState == AttackButtonState.Both ? AttackButtonState.Left : AttackButtonState.None;
        StopAttacking();
    }

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
    }
    private IEnumerator ShieldCoroutine()
    {
        shield.gameObject.SetActive(true);
        moveSpeed = 0;
        yield return new WaitForSeconds(shieldDuration);
        if (isAttacking)
        {
            moveSpeed = maxMoveSpeed * .25f;
        }
        else
        {
            moveSpeed = maxMoveSpeed;
        }
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
            //Debug.Log("Skill is on cooldown.");
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
        public Transform shooter;
    }

    [SerializeField] private AttackParameters mainAttackParameters;
    [SerializeField] private AttackParameters secondaryAttackParameters;
    [SerializeField] private AttackParameters createWaterParameters;
    [SerializeField] private AnimationControlScript animationControlScript;

    //make a variable to present if only left mouse button is pressed or right mouse button is pressed or both
    private enum AttackButtonState
    {
        None,
        Left,
        Right,
        Both
    }

    private AttackButtonState buttonState;

    private void PerformAttack(AttackParameters parameters)
    {
        Vector3 cursorPosition = GetCursorPointOnGround();
        cursorPosition.y = 0.3f;
        if (cursorPosition != Vector3.zero) // Ensure we have a valid point
        {
            Vector3 lookDirection = (cursorPosition - parameters.shooter.position).normalized;
            parameters.shooter.forward = lookDirection;
        }
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

    private float _createWaterCooldown = 1f;
    private IEnumerator CreateWater()
    {
        while (true)
        {
            // Create a water prefab from Instantiate Manager every 1 seconds 
            if (_createWaterCooldown <= 0)
            {
                InstantiateManager.Instance.InstantiateWater(transform.position + transform.forward * 2.5f);
                _createWaterCooldown = 1f;
            }
            yield return new WaitForSeconds(.1f);
            _createWaterCooldown -= .1f;
        }
    }

    private void StartAttacking()
    {
        animationControlScript.SetAimingLayerWeight(1f);
        if (buttonState == AttackButtonState.Both)
        {
            StopAttacking();
            createWaterParameters.effect.Play();
            _createWaterCooldown = 1f;
            currentAttackCoroutine = StartCoroutine(CreateWater());
        }
        else if (buttonState == AttackButtonState.Left)
        {
            isAttacking = true;
            moveSpeed = maxMoveSpeed * .25f;
            AdjustAttackEffect(mainAttackParameters);
            mainAttackParameters.effect.Play();
            currentAttackCoroutine = StartCoroutine(AttackCoroutine(mainAttackParameters));
        }
        else if (buttonState == AttackButtonState.Right)
        {
            isAttacking = true;
            moveSpeed = maxMoveSpeed * .25f;
            AdjustAttackEffect(secondaryAttackParameters);
            secondaryAttackParameters.effect.Play();
            currentAttackCoroutine = StartCoroutine(AttackCoroutine(secondaryAttackParameters));
        }

        // if (currentAttackCoroutine == null)
        // {
        //     isAttacking = true;
        //     moveSpeed *= .25f;
        //     lastParameters = parameters;
        //     AdjustAttackEffect(parameters);
        //     parameters.effect.Play();
        //     currentAttackCoroutine = StartCoroutine(AttackCoroutine(parameters));
        // }
        // else
        // {
        //     StopAttacking(lastParameters);
        //     StartAttacking(parameters);
        // }
    }

    private void StopAttacking()
    {
        if (buttonState == AttackButtonState.None)
        {
            isAttacking = false;
            moveSpeed = maxMoveSpeed;
            StopCoroutine(currentAttackCoroutine);
            secondaryAttackParameters.effect.Stop();
            mainAttackParameters.effect.Stop();
            currentAttackCoroutine = null;
            animationControlScript.SetAimingLayerWeight(0f);
        }
        else if (buttonState == AttackButtonState.Left)
        {
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
                createWaterParameters.effect.Stop();
                currentAttackCoroutine = null;
            }
            StartAttacking();
        }
        else if (buttonState == AttackButtonState.Right)
        {
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
                createWaterParameters.effect.Stop();
                currentAttackCoroutine = null;
            }
            StartAttacking();
        }
        else
        {
            if (currentAttackCoroutine != null)
            {
                StopCoroutine(currentAttackCoroutine);
                currentAttackCoroutine = null;
            }
            secondaryAttackParameters.effect.Stop();
            mainAttackParameters.effect.Stop();
        }

        // if (currentAttackCoroutine != null && lastParameters.effect == parameters.effect)
        // {
        //     isAttacking = false;
        //     moveSpeed = maxMoveSpeed;
        //     StopCoroutine(currentAttackCoroutine);
        //     parameters.effect.Stop();
        //     currentAttackCoroutine = null;
        // }
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
            bool canDash = !Physics.BoxCast(transform.position + Vector3.up, Vector3.one * 0.5f, moveDir,
                out hitInfo, Quaternion.identity, dashDistance, collisionLayerMask);
            Debug.Log("Can dash: " + canDash);
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
        bool canMove = !Physics.BoxCast(transform.position + Vector3.up, Vector3.one * .5f,
            moveDir, Quaternion.identity, playerRadius, collisionLayerMask);
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
        OnDeath?.Invoke(this, EventArgs.Empty);
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
                //check if y difference is less than 3
                if (Mathf.Abs(closestPoint.y - attacker.position.y) > 3) continue;
                Vector3 directionToTarget = (closestPoint - attacker.position).normalized;
                // Project the attacker's forward direction onto the XZ plane
                Vector3 attackerForwardXZ = new Vector3(attacker.forward.x, 0, attacker.forward.z).normalized;

                // Project the direction to the target onto the XZ plane
                Vector3 directionToTargetXZ = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;

                // Calculate the angle on the XZ plane
                float angleToTargetXZ = Vector3.Angle(attackerForwardXZ, directionToTargetXZ);
                if (angleToTargetXZ <= parameters.angle)
                {
                    IDamageable.Damage damage = new IDamageable.Damage(parameters.damage, parameters.elementType, Instance.transform);
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

    public Vector3 GetPlayerPositionOnPlane()
    {
        Vector3 ret = transform.position;
        ret.y = 0;
        return ret;
    }
}