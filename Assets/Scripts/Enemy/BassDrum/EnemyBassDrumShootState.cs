using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBassDrumShootState : EnemyState
{
    private int _bulletsToShoot;
    private float _shootingTimer;
    private int _currentBulletIndex;
    private readonly List<float> timeSpace;
    private bool CanShoot => _shootingTimer <= 0;
    private readonly EnemyBassDrum _enemyBassDrum;

    public EnemyBassDrumShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

        _enemyBassDrum = EnemyStateMachine.Enemy as EnemyBassDrum;
        timeSpace = _enemyBassDrum?.timeSpace;
    }

    public override void Enter()
    {
        base.Enter();
        _bulletsToShoot = timeSpace.Count + 1;

        _enemyBassDrum.TotalBulletsShot = _bulletsToShoot;
        _enemyBassDrum.ReflectedBullets = 0;

        _currentBulletIndex = 0;
        _shootingTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (_bulletsToShoot <= 0 && _enemyBassDrum != null)
        {
            EnemyStateMachine.ChangeState(_enemyBassDrum.TrackPlayerState);
            return;
        }


        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }


        if (CanShoot && _bulletsToShoot > 0)
        {
            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab,
                EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f,
                Quaternion.identity);
            bullet.GetComponent<Bullet>().SetBulletProperty(direction, 5, 10f);
            _enemyBassDrum.RegisterBullet(bullet.GetComponent<Bullet>());
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

    public override void Exit()
    {
        base.Exit();
    }
}