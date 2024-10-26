using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpiralShootState : EnemyState
{
    private int _projectilePerShoot = 2;
    private int _maxBullets = 5;
    private int _bulletsToShoot = 5;
    private float _shootingTimer = 0f; // Cooldown timer
    private float _loadBulletTimer = 0f;
    private bool CanShoot => _shootingTimer <= 0 && _loadBulletTimer <= 0;

    public EnemySpiralShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy Spiral Shoot Enter");
        _bulletsToShoot = _maxBullets;
    }
    public override void Update()
    {
        base.Update();
        EnemySpiral spiralEnemy = EnemyStateMachine.Enemy as EnemySpiral;
        // if (_bulletsToShoot <= 0 && spiralEnemy != null)
        // {
        //     EnemyStateMachine.ChangeState(spiralEnemy.TrackPlayerState);
        // }

        EnemyStateMachine.Enemy.transform.GetPositionAndRotation(out var enemyPosition, out var rotation);
        Vector3 playerPosition = Player.Instance.transform.position;
        EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);

        if (_loadBulletTimer > 0)
        {
            _loadBulletTimer -= Time.deltaTime;
        }
        else if (_bulletsToShoot <= 0)
        {
            _loadBulletTimer = .5f;
            _bulletsToShoot = _maxBullets;
        }

        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }

        // if (_shootingTimer > 0)
        // {
        //     _shootingTimer -= Time.deltaTime;
        // }
        // else
        // {
        //     _shootingTimer = .1f;
        // }

        if (CanShoot)
        {
            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            Vector3 center = enemyPosition + Vector3.up * 0.75f;
            float theta = 360f / _projectilePerShoot;
            for (int i = 0; i < _projectilePerShoot; i++)
            {
                Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab, center, rotation);
                // Set bullet direction
                bullet.GetComponent<BulletSpiral>().SetBulletProperty(direction, 10, 10f);
                bullet.GetComponent<BulletSpiral>().SetSpiralProperty(center, (90 + theta * i + rotation.eulerAngles.z) % 360f);
            }
            _bulletsToShoot--;
            _shootingTimer = .5f;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}