using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretShootState : EnemyState
{
    private int _bulletsToShoot = 1;
    private float _shootingTimer = 0f; // Cooldown timer
    private bool CanShoot => _shootingTimer <= 0;

    public EnemyTurretShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _bulletsToShoot = 1;
    }

    public override void Update()
    {
        
        base.Update();
        EnemyTurret turretEnemy = EnemyStateMachine.Enemy as EnemyTurret;
        if (turretEnemy != null)
        {
            
            if (_bulletsToShoot <= 0)
            {
                EnemyStateMachine.ChangeState(turretEnemy.TrackPlayerState);
            }

            if (_shootingTimer > 0)
            {
                _shootingTimer -= Time.deltaTime;
            }
            else
            {
                _shootingTimer = .1f; // 控制射擊頻率

                // 發射子彈
                Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
                Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab, EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetBulletProperty(direction, 10, 10f);
                _bulletsToShoot--;
            }
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
