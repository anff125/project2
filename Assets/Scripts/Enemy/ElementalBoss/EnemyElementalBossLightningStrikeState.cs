using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossLightningStrikeState : EnemyState
{
    private EnemyElementalBoss _bossEnemy;
    private Player _player;
    private LineRenderer _circleRenderer;
    private Vector3 _targetPosition;
    private float _circleRadius;
    private const float MAX_RADIUS = 5f;
    private const float EXPANSION_RATE = 4f;
    private const int CIRCLE_SEGMENTS = 50;
    private const float DAMAGE_INTERVAL = 0.2f;

    private float _damageTimer;

    public EnemyElementalBossLightningStrikeState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
    }

    public override void Enter()
    {
        base.Enter();
        _player = Player.Instance;

        if (_bossEnemy == null || _player == null)
        {
            Debug.LogError("Boss or Player is null");
            return;
        }

        _targetPosition = _player.transform.position;

        _bossEnemy.damageIndicatorPrefab.position = _targetPosition;
        _bossEnemy.damageIndicatorPrefab.localScale = new Vector3(0, .01f, 0);
        _bossEnemy.damageIndicatorPrefab.gameObject.SetActive(true);

        _circleRadius = 0;
        CreateCircleRenderer();
    }

    public override void Update()
    {
        base.Update();

        float scaleSpeed = 14f;
        Vector3 targetScale = new Vector3(MAX_RADIUS, 0.01f, MAX_RADIUS);
        _bossEnemy.damageIndicatorPrefab.localScale = Vector3.MoveTowards(
            _bossEnemy.damageIndicatorPrefab.localScale,
            targetScale,
            scaleSpeed * Time.deltaTime
        );

        if (Mathf.Approximately(_bossEnemy.damageIndicatorPrefab.localScale.x, MAX_RADIUS))
        {
            _bossEnemy.damageIndicatorPrefab.gameObject.SetActive(false);
        }

        else
        {
            return;
        }
        
        float moveSpeed = 30f;
        _bossEnemy.transform.position = Vector3.MoveTowards(
            _bossEnemy.transform.position,
            _targetPosition,
            moveSpeed * Time.deltaTime
        );
        
        if (Vector3.Distance(_bossEnemy.transform.position, _targetPosition) < 0.001f)
        {
            _circleRenderer.startColor = Color.cyan;
            _circleRenderer.endColor = Color.cyan;
            
            _circleRadius += EXPANSION_RATE * Time.deltaTime;
            UpdateCircleRenderer();

            _damageTimer += Time.deltaTime;
            if (_damageTimer >= DAMAGE_INTERVAL)
            {
                DealDamage();
                _damageTimer = 0f;
            }

            if (_circleRadius >= MAX_RADIUS * 2)
            {
                EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseBaseState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        // 清理 LineRenderer
        if (_circleRenderer != null)
        {
            Object.Destroy(_circleRenderer.gameObject);
        }
    }

    private void CreateCircleRenderer()
    {
        GameObject circleObject = new GameObject("LightningStrikeCircle");
        _circleRenderer = circleObject.AddComponent<LineRenderer>();

        _circleRenderer.positionCount = CIRCLE_SEGMENTS + 1;
        _circleRenderer.useWorldSpace = false;
        _circleRenderer.startWidth = 0.1f;
        _circleRenderer.endWidth = 0.1f;
        _circleRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _circleRenderer.startColor = Color.red;
        _circleRenderer.endColor = Color.red;

        circleObject.transform.position = _targetPosition;

        Vector3[] positions = new Vector3[CIRCLE_SEGMENTS + 1];
        for (int i = 0; i <= CIRCLE_SEGMENTS; i++)
        {
            float angle = i * 360f / CIRCLE_SEGMENTS;
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * MAX_RADIUS * .5f;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * MAX_RADIUS * .5f;
            positions[i] = new Vector3(x, 0, z);
        }

        _circleRenderer.SetPositions(positions);
        _circleRenderer.transform.position = _targetPosition;
    }

    private void UpdateCircleRenderer()
    {
        if (_circleRenderer == null) return;

        Vector3[] positions = new Vector3[CIRCLE_SEGMENTS + 1];
        for (int i = 0; i <= CIRCLE_SEGMENTS; i++)
        {
            float angle = i * 360f / CIRCLE_SEGMENTS;
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _circleRadius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * _circleRadius;
            positions[i] = new Vector3(x, 0, z);
        }

        _circleRenderer.SetPositions(positions);
        _circleRenderer.transform.position = _targetPosition;
    }

    private void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_targetPosition, _circleRadius, LayerMask.GetMask("Player", "Wire"));
        float edgeThickness = 0.5f; // Define the thickness of the circle's edge

        foreach (var collider in hitColliders)
        {
            Vector3 closestPoint = collider.ClosestPoint(_targetPosition);
            float distanceToCenter = Vector3.Distance(_targetPosition, closestPoint);

            // Check if the object is within the edge range
            if (distanceToCenter >= _circleRadius - edgeThickness && distanceToCenter <= _circleRadius)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    IDamageable.Damage damage = new IDamageable.Damage(10, ElementType.Electric, _bossEnemy.transform);
                    damageable.TakeDamage(damage);
                }
            }
        }
    }

}