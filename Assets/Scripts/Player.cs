using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private int maxHealth;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float mainAttackCooldown;
    [SerializeField] private float secondaryAttackCooldown;
    [SerializeField] private float dashCooldown;
    [SerializeField] private Transform drumDetector;

    private Renderer drumDetectorRenderer;

    private float mainAttackTimer;
    private bool mainAttackOnCooldown;
    private float secondaryAttackTimer;
    private bool secondaryAttackOnCooldown;
    private float dashTimer;
    private bool dashOnCooldown;

    private const float RADIUS = 2f;
    private const float BULLET_RADIUS = 0.5f;

    private float _currentHealth;
    public static Player Instance { get; private set; }
    // private float _currentMana;
    private bool _isMoving;
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;

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
        GameInput.Instance.OnMainAttack += OnMainAttack;
        GameInput.Instance.OnMainAttackCancelled += OnMainAttackCancelled;
        GameInput.Instance.OnSecondaryAttackStarted += OnSecondaryAttackStarted;
        GameInput.Instance.OnSecondaryAttack += OnSecondaryAttack;
        GameInput.Instance.OnSecondaryAttackCancelled += OnSecondaryAttackCancelled;
        GameInput.Instance.OnDash += OnDash;
        lineRenderer.positionCount = 0;
        DrawHollowCircle(RADIUS, Color.white);

        //set drum detector scale x,z to RADIUS*2
        drumDetector.localScale = new Vector3(RADIUS * 2, .01f, RADIUS * 2);
        drumDetectorRenderer = drumDetector.GetComponent<Renderer>();
    }


    private void Update()
    {
        HandleMovement();

        // Update cooldown timers
        if (mainAttackOnCooldown)
        {
            mainAttackTimer -= Time.deltaTime;
            if (mainAttackTimer <= 0)
            {
                mainAttackOnCooldown = false;
            }
            // Set circle color to grey while on cooldown
            SetLineColor(Color.grey);
        }
        else
        {
            // Set circle color to white when ready
            SetLineColor(Color.cyan);
        }

        if (secondaryAttackOnCooldown)
        {
            secondaryAttackTimer -= Time.deltaTime;
            if (secondaryAttackTimer <= 0)
            {
                secondaryAttackOnCooldown = false;
            }
            drumDetectorRenderer.material.color = Color.grey;
        }
        else
        {
            drumDetectorRenderer.material.color = Color.red;
        }

        if (dashOnCooldown)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                dashOnCooldown = false;
            }
        }
    }
    private void OnMainAttackCancelled(object sender, EventArgs e) { }
    private void OnSecondaryAttackStarted(object sender, EventArgs e) { StartSecondaryAttacking(); }
    private void OnSecondaryAttackCancelled(object sender, EventArgs e) { StopSecondaryAttacking(); }

    private bool isAttacking;

    private void StartSecondaryAttacking() { }

    private void StopSecondaryAttacking() { }

    // Method to trigger main attack

    private void OnDash(object sender, EventArgs e)
    {
        if (!dashOnCooldown)
        {
            // Execute dash logic, move the player forward
            Vector3 dashDirection = transform.forward;
            float dashDistance = 5f; // Adjust this value to control dash distance
            transform.position += dashDirection * dashDistance;
            dashOnCooldown = true;
            dashTimer = dashCooldown;
        }
        else
        {
            Debug.Log("Dash is on cooldown.");
        }
    }
    private void DrawHollowCircle(float radius, Color color)
    {
        int segments = 360;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false; // Draw the circle relative to the player

        // Set the color of the circle
        SetLineColor(color);

        // Adjust the line width to ensure it's thin and hollow
        lineRenderer.startWidth = BULLET_RADIUS;
        lineRenderer.endWidth = BULLET_RADIUS;

        float angle = 0f;
        for (int i = 0; i < segments + 1; i++)
        {
            // Calculate positions around the circle using Sin and Cos functions
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0.01f, z)); // Set position on XZ plane
            angle += (360f / segments); // Increment angle for next point
        }
    }
    private void OnMainAttack(object sender, EventArgs e)
    {
        if (mainAttackOnCooldown)
        {
            return;
        }

        SetLineColor(Color.yellow);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.3f,
            RADIUS + BULLET_RADIUS, LayerMask.GetMask("Bullet"));

        bool bulletRebounded = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("BulletHiHat"))
            {
                float bulletEdgeDistance = Vector3.Distance(transform.position, hitCollider.transform.position);

                if (bulletEdgeDistance >= RADIUS - BULLET_RADIUS && bulletEdgeDistance <= RADIUS + BULLET_RADIUS)
                {
                    var bullet = hitCollider.GetComponent<Bullet>();
                    // Change the color of the bullet to yellow
                    bullet.SetTextureForPlayer();

                    // Shoot the bullet in the direction of player to the bullet, y=0
                    Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                    direction.y = 0; // Ensure no vertical component in the direction
                    bullet.SetShooterLayerMask(LayerMask.GetMask("Player"));
                    bullet.SetBulletProperty(direction, 10, 10f, 10);
                    bullet.ReflectBullet();
                    bulletRebounded = true;
                }
            }
        }

        if (!bulletRebounded)
        {
            mainAttackOnCooldown = true;
            mainAttackTimer = mainAttackCooldown;
        }

    }

    private void OnSecondaryAttack(object sender, EventArgs e)
    {
        if (secondaryAttackOnCooldown)
        {
            return;
        }

        drumDetectorRenderer.material.color = Color.yellow;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.3f,
            RADIUS - BULLET_RADIUS, LayerMask.GetMask("Bullet"));
        bool bulletRebounded = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("BulletDrum"))
            {
                var bullet = hitCollider.GetComponent<Bullet>();
                // Change the color of the bullet to yellow
                bullet.SetTextureForPlayer();
                // Shoot the bullet in the direction of player to the bullet, y=0
                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                direction.y = 0; // Ensure no vertical component in the direction
                bullet.SetShooterLayerMask(LayerMask.GetMask("Player"));
                bullet.SetBulletProperty(direction, 10, 10f, 10);
                bullet.ReflectBullet();
                bulletRebounded = true;
            }
        }

        if (!bulletRebounded)
        {
            secondaryAttackOnCooldown = true;
            secondaryAttackTimer = secondaryAttackCooldown;
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

    private void SetLineColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}