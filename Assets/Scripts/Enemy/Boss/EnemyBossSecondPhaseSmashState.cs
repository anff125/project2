using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSecondPhaseSmashState : EnemyState
{
    public EnemyBossSecondPhaseSmashState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss _bossEnemy;
    private LineRenderer _circleRenderer;
    private Vector3 _lastPlayerPosition;
    private const float CIRCLE_RADIUS = 2.5f;
    private const int CIRCLE_SEGMENTS = 50;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        if (_bossEnemy == null)
        {
            Debug.LogError("Enemy is not a boss");
            return;
        }

        // Save the player's position when entering the state
        _lastPlayerPosition = Player.Instance.transform.position;


        //move boss above the player
        _bossEnemy.transform.position = _lastPlayerPosition + Vector3.up * 5;
        _bossEnemy.smashPrefab.position = _lastPlayerPosition;
        _bossEnemy.smashPrefab.localScale = new Vector3(0, .01f, 0);
        _bossEnemy.smashPrefab.gameObject.SetActive(true);
        // 創建並設置 LineRenderer
        CreateCircleRenderer();
        // 在玩家位置繪製圓圈
        DrawCircleAtPlayerPosition();
    }


    public override void Update()
    {
        base.Update();

        //gradually increase the size of the smash prefab
        _bossEnemy.smashPrefab.localScale = Vector3.Lerp(_bossEnemy.smashPrefab.localScale,
            new Vector3(CIRCLE_RADIUS, .01f, CIRCLE_RADIUS), Time.deltaTime);
        //if the smash prefab is big enough, smash the player
        if (_bossEnemy.smashPrefab.localScale.x >= CIRCLE_RADIUS - 0.1f)
        {
            DealDamage();
            //move boss to y = 0
            _bossEnemy.transform.position = new Vector3(_lastPlayerPosition.x, 0, _lastPlayerPosition.z);
            //change state to idle
            EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseIdleState);
        }
        else if (_bossEnemy.smashPrefab.localScale.x >= CIRCLE_RADIUS - 0.3f) { }
        else
        {
            _lastPlayerPosition = Player.Instance.transform.position;
            _bossEnemy.transform.position = _lastPlayerPosition + Vector3.up * 5;
            DrawCircleAtPlayerPosition();
        }
    }
    private void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_lastPlayerPosition, CIRCLE_RADIUS * _bossEnemy.transform.localScale.x * 0.5f,
            LayerMask.GetMask("Enemy", "Player"));
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.gameObject.name);
            //if enemy destroy it, exclude the boss
            if (hitCollider.gameObject != _bossEnemy.gameObject && hitCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Object.Destroy(hitCollider.gameObject);
            }
            //if player deal damage
            else if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                IDamageable.Damage damage = new IDamageable.Damage(50, ElementType.Physical, _bossEnemy.transform);
                hitCollider.GetComponent<IDamageable>()?.TakeDamage(damage);
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
        // 創建一個新的 GameObject 來持有 LineRenderer
        GameObject circleObject = new GameObject("SmashCircle");
        circleObject.transform.localScale = new Vector3(CIRCLE_RADIUS, .1f, CIRCLE_RADIUS);
        // 設置 LineRenderer 的父物件為 Enemy
        circleObject.transform.SetParent(EnemyStateMachine.Enemy.transform);
        //Set the position to the player position
        circleObject.transform.position = _lastPlayerPosition;
        //Set the scale to the circle radius

        _circleRenderer = circleObject.AddComponent<LineRenderer>();

        // 設置 LineRenderer 的屬性
        _circleRenderer.positionCount = CIRCLE_SEGMENTS + 1;
        _circleRenderer.useWorldSpace = false;
        _circleRenderer.startWidth = 0.1f;
        _circleRenderer.endWidth = 0.1f;
        _circleRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _circleRenderer.startColor = Color.red;
        _circleRenderer.endColor = Color.red;
    }

    private void DrawCircleAtPlayerPosition()
    {
        if (_lastPlayerPosition == null || _circleRenderer == null) return;

        Vector3[] positions = new Vector3[CIRCLE_SEGMENTS + 1];
        for (int i = 0; i <= CIRCLE_SEGMENTS; i++)
        {
            float angle = i * 360f / CIRCLE_SEGMENTS;
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * CIRCLE_RADIUS;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * CIRCLE_RADIUS;
            positions[i] = new Vector3(x, 0, z);
        }

        _circleRenderer.SetPositions(positions);
        _circleRenderer.transform.position = _lastPlayerPosition;
    }
}