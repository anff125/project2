using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossIceBlockLaserState : EnemyState
{
    private EnemyElementalBoss _bossEnemy;
    private LineRenderer _laserLine;
    private Vector3 _laserDirection;
    private float _blinkTimer = 0;
    private bool _isBlinking = true;
    private readonly float _laserDuration = 3f; // Total laser active time
    private const float BLINK_INTERVAL = 0.2f; // Interval for laser blinking

    private const float BLINK_DURATION = 2f; // Duration for blinking
    private const float INITIAL_LASER_WIDTH = 0.5f;
    private const float FINAL_LASER_WIDTH = 3f;
    private const float LASER_LENGTH = 20f;

    public EnemyElementalBossIceBlockLaserState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
    }

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(_laserDuration);

        // Instantiate the IceBlock at the player's position, facing the boss
        Vector3 playerPosition = Player.Instance.GetPlayerPositionOnPlane();
        Vector3 directionToBoss = (_bossEnemy.transform.position - playerPosition).normalized;

        Object.Instantiate(_bossEnemy.iceBlockPrefab, playerPosition, Quaternion.LookRotation(directionToBoss));

        // Prepare the laser line renderer
        GameObject laserObj = new GameObject("IceLaser");
        //rotate laserObj x 90 degrees
        laserObj.transform.Rotate(90, 0, 0);
        _laserLine = laserObj.AddComponent<LineRenderer>();
        _laserLine.alignment = LineAlignment.TransformZ;
        _laserLine.startWidth = INITIAL_LASER_WIDTH;
        _laserLine.endWidth = INITIAL_LASER_WIDTH;
        _laserLine.startColor = Color.cyan;
        _laserLine.endColor = Color.cyan;
        _laserLine.material = new Material(Shader.Find("Sprites/Default"));

        _isBlinking = true;
        _blinkTimer = 0;
        // Set laser direction toward the player
        _laserDirection = (Player.Instance.transform.position - _bossEnemy.transform.position).normalized;
        FireLaser();
    }

    public override void Update()
    {
        base.Update();

        if (_isBlinking)
        {
            _blinkTimer += Time.deltaTime;

            // Toggle laser visibility every BLINK_INTERVAL
            if (_blinkTimer >= BLINK_INTERVAL)
            {
                ToggleLaserVisibility();
                _blinkTimer = 0;
            }

            // Stop blinking and enlarge the laser after BLINK_DURATION
            if (TimeInCurrentState >= BLINK_DURATION)
            {
                _isBlinking = false;
                EnlargeLaser();
            }
        }
        else
        {
            // Check if the player is in the laser's path after enlarging
            if (Physics.BoxCast(_bossEnemy.transform.position,
                    Vector3.one * (FINAL_LASER_WIDTH * 0.5f),
                    _laserDirection, out RaycastHit hit, Quaternion.LookRotation(_laserDirection),
                    LASER_LENGTH, LayerMask.GetMask("Player")))
            {
                IDamageable.Damage damage = new IDamageable.Damage(0.5f, ElementType.Ice, _bossEnemy.transform);
                Player.Instance.TakeDamage(damage);
            }

            // Transition to next state after the laser duration
            if (TimeInCurrentState >= _laserDuration)
            {
                EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
            }
        }
    }

    private void ToggleLaserVisibility()
    {
        _laserLine.enabled = !_laserLine.enabled;
    }

    private void FireLaser()
    {
        Vector3 laserStart = _bossEnemy.transform.position;
        Vector3 laserEnd = laserStart + _laserDirection * LASER_LENGTH;
        laserStart.y = .1f;
        laserEnd.y = .1f;
        _laserLine.SetPosition(0, laserStart);
        _laserLine.SetPosition(1, laserEnd);
        _laserLine.enabled = true;
    }

    private void EnlargeLaser()
    {
        // Stop blinking and enlarge the laser width
        _laserLine.startWidth = FINAL_LASER_WIDTH;
        _laserLine.endWidth = FINAL_LASER_WIDTH;
        _laserLine.enabled = true;
    }

    public override void Exit()
    {
        base.Exit();
        EnemyStateMachine.EnemyElementalBoss.meleeCount = 0;
        // Clean up the laser and ice block
        if (_laserLine != null) GameObject.Destroy(_laserLine.gameObject);
    }
}