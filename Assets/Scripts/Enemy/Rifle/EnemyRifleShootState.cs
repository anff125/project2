using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifleShootState : EnemyState
{
    private int _bulletsToShoot = 5;
    private float _shootingTimer = 0f; // Cooldown timer
    private bool CanShoot => _shootingTimer <= 0;

    public EnemyRifleShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy Rifle Shoot Enter");
        _bulletsToShoot = 5;
    }
    public override void Update()
    {
        base.Update();
        EnemyRifle rifleEnemy = EnemyStateMachine.Enemy as EnemyRifle;
        if (_bulletsToShoot <= 0 && rifleEnemy != null)
        {
            EnemyStateMachine.ChangeState(rifleEnemy.TrackPlayerState);
        }

        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }
        else
        {
            _shootingTimer = .1f;
        }

        if (CanShoot)
        {
            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab, EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f, Quaternion.identity);
            // Set bullet direction
            bullet.GetComponent<Bullet>().SetBulletProperty(direction, 10, 10f);
            _bulletsToShoot--;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}