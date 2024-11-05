using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireShootState : EnemyState
{
    private int _bulletsToShoot = 5;
    private float _shootingTimer = 0f; // Cooldown timer
    private readonly float _shootingInterval; // Interval between shots
    private bool CanShoot => _shootingTimer <= 0;

    // Add the shooting interval as a parameter in the constructor
    public EnemyFireShootState(EnemyStateMachine enemyStateMachine, float shootingInterval = 0.2f) : base(enemyStateMachine)
    {
        _shootingInterval = shootingInterval;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered EnemyFireShootState");
        _bulletsToShoot = 5;
    }

    public override void Update()
    {
        base.Update();
        EnemyFire fireEnemy = EnemyStateMachine.Enemy as EnemyFire;
        if (_bulletsToShoot <= 0 && fireEnemy != null)
        {
            EnemyStateMachine.ChangeState(fireEnemy.TrackPlayerState);
        }

        // Handle the cooldown timer
        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }

        // Shoot if the timer has reached 0 (CanShoot) and reset the timer
        if (CanShoot)
        {   
            Debug.Log("EnemyFire attempting to shoot bullet...");
            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab,
                EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f,
                Quaternion.identity);
            // Set bullet properties
            bullet.GetComponent<BulletWavy>().SetBulletWavyProperty(direction, 5, 10f);

            _bulletsToShoot--;
            _shootingTimer = _shootingInterval; // Reset the timer to the shooting interval
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}