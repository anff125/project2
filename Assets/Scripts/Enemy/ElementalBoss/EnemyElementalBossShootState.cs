using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossShootState : EnemyState
{
    private int _bulletsToShoot;
    private float _shootingTimer;
    private int _currentBulletIndex;
    private readonly List<float> timeSpace;
    private bool CanShoot => _shootingTimer <= 0;
    private readonly EnemyElementalBoss _bossEnemy;

    private int toggle = 1;
    public EnemyElementalBossShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
        timeSpace = _bossEnemy?.secondPhaseTimeSpace;
    }

    public override void Enter()
    {
        base.Enter();
        // float angle = Random.Range(0f, 360f);
        // Vector3 randomPosition = Player.Instance.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * 10f;
        //_enemyElementalBoss.StartCoroutine(_enemyElementalBoss.MoveToPosition(randomPosition));

        _bulletsToShoot = timeSpace.Count + 1;

        _bossEnemy.TotalBulletsShot = _bulletsToShoot;
        _bossEnemy.ReflectedBullets = 0;

        _currentBulletIndex = 0;
        _shootingTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (_bulletsToShoot <= 0 && _bossEnemy != null)
        {
            if (_bossEnemy.isSecondPhase)
            {
                EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseBaseState);
            }
            else
            {
                EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
            }
            return;
        }

        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }

        if (CanShoot && _bulletsToShoot > 0)
        {
            ShootBulletRazorLeafFollowTarget();
            // if (_bossEnemy.isSecondPhase)
            // {
            //     Vector3 directionToPlayer = Vector3.Normalize(Player.Instance.transform.position - _bossEnemy.transform.position);
            //     Vector3 targetPosition = Player.Instance.transform.position + directionToPlayer * 10;
            //     ShootBulletRazorLeafFollowPosition(targetPosition);
            // }

            _bulletsToShoot--;

            if (_currentBulletIndex < timeSpace.Count)
            {
                _shootingTimer = timeSpace[_currentBulletIndex];
                _currentBulletIndex++;
            }
            else
            {
                _shootingTimer = 0f;
            }
        }
    }

    private void ShootBulletRazorLeafFollowTarget()
    {
        // Instantiate BulletRazorLeaf at the enemy's position
        Transform bulletObject = Object.Instantiate(_bossEnemy.bulletRazorLeafPrefab,
            _bossEnemy.transform.position + Vector3.up * 0.3f,
            Quaternion.identity);
        var bullet = bulletObject.GetComponent<BulletRazorLeaf>();
        bullet.SetToggle(toggle);
        toggle *= -1;
        bullet.SetBulletProperty(Vector3.forward, 10, 10, 25);
        _bossEnemy.RegisterBullet(bullet);
    }
    private void ShootBulletRazorLeafFollowPosition(Vector3 targetPosition)
    {
        // Instantiate BulletRazorLeaf at the enemy's position
        Transform bulletObject = Object.Instantiate(
            _bossEnemy.bulletRazorLeafPrefab,
            _bossEnemy.transform.position + Vector3.up * 0.3f,
            Quaternion.identity
        );

        var bullet = bulletObject.GetComponent<BulletRazorLeaf>();

        // Set the bullet to use FollowUntilPosition behavior
        bullet.SetBulletType(BulletRazorLeaf.BulletType.FollowUntilPosition);

        // Set the target position for the bullet to follow
        bullet.SetTargetPosition(targetPosition);

        // Set initial direction and other properties
        bullet.SetToggle(toggle);
        toggle *= -1;

        bullet.SetBulletProperty(Vector3.forward, 10, 7, 25);

        // Register the bullet with the boss enemy if needed
        _bossEnemy.RegisterBullet(bullet);
    }

    public override void Exit()
    {
        base.Exit();
        EnemyStateMachine.EnemyElementalBoss.meleeCount = 0;

    }
}