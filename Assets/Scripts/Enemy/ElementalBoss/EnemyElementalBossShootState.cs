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
    private readonly EnemyElementalBoss _enemyElementalBoss;

    private int toggle = 1;
    public EnemyElementalBossShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _enemyElementalBoss = EnemyStateMachine.Enemy as EnemyElementalBoss;
        timeSpace = _enemyElementalBoss?.timeSpace;
    }

    public override void Enter()
    {
        base.Enter();
        float angle = Random.Range(0f, 360f);
        Vector3 randomPosition = Player.Instance.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * 10f;
        _enemyElementalBoss.transform.position = randomPosition;

        _bulletsToShoot = timeSpace.Count + 1;

        _enemyElementalBoss.TotalBulletsShot = _bulletsToShoot;
        _enemyElementalBoss.ReflectedBullets = 0;

        _currentBulletIndex = 0;
        _shootingTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (_bulletsToShoot <= 0 && _enemyElementalBoss != null)
        {
            EnemyStateMachine.ChangeState(_enemyElementalBoss.TrackPlayerState);
            return;
        }

        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }

        if (CanShoot && _bulletsToShoot > 0)
        {
            ShootBulletRazorLeaf();

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

    private void ShootBulletRazorLeaf()
    {
        // Instantiate BulletRazorLeaf at the enemy's position
        Transform bulletObject = Object.Instantiate(_enemyElementalBoss.bulletRazorLeafPrefab,
            _enemyElementalBoss.transform.position + Vector3.up * 0.3f,
            Quaternion.identity);
        var bullet = bulletObject.GetComponent<BulletRazorLeaf>();
        bullet.SetToggle(toggle);
        toggle *= -1;
        bullet.SetBulletProperty(Vector3.forward, 10, 7, 25);
        _enemyElementalBoss.RegisterBullet(bullet);
    }

    public override void Exit()
    {
        base.Exit();
        EnemyStateMachine.EnemyElementalBoss.meleeCount = 0;

    }
}