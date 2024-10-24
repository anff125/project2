using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrumShootState : EnemyState
{
    private int _bulletsToShoot;
    private float _shootingTimer = 0f; // Cooldown timer
    private int _currentBulletIndex = 0;
    List<float> timeSpace; // Time intervals between each bullet shot, customizable in Unity Editor
    private bool CanShoot => _shootingTimer <= 0;
    private readonly EnemyDrum enemyDrum;
    // Remove the shooting interval parameter from the constructor since we're using timespace from the editor
    public EnemyDrumShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        // Optionally, you can initialize timeSpace here if you want to give default values
        enemyDrum = EnemyStateMachine.Enemy as EnemyDrum;
        timeSpace = enemyDrum?.timeSpace;
    }

    public override void Enter()
    {
        base.Enter();
        _bulletsToShoot = timeSpace.Count + 1; // Number of bullets to shoot is based on timespace length
        enemyDrum.totalBulletsShot = _bulletsToShoot; // Reset the total bullets shot
        _currentBulletIndex = 0;
        _shootingTimer = 0f; // Initialize the shooting timer
    }

    public override void Update()
    {
        base.Update();

        if (_bulletsToShoot <= 0 && enemyDrum != null)
        {
            EnemyStateMachine.ChangeState(enemyDrum.TrackPlayerState);
            return; // Exit Update early if no more bullets to shoot
        }

        // Handle the cooldown timer
        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }

        // Shoot if the timer has reached 0 (CanShoot) and reset the timer
        if (CanShoot && _bulletsToShoot > 0)
        {
            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab,
                EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f,
                Quaternion.identity);
            // Set bullet properties
            bullet.GetComponent<Bullet>().SetBulletProperty(direction, 5, 10f);
            enemyDrum.RegisterBullet(bullet.GetComponent<Bullet>()); // Register the bullet with the specific enemy that shot it
            _bulletsToShoot--;

            // Set the next shooting interval based on the timespace list
            if (_currentBulletIndex < timeSpace.Count)
            {
                _shootingTimer = timeSpace[_currentBulletIndex];
                _currentBulletIndex++;
            }
            else
            {
                _shootingTimer = 0f; // No more bullets left
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}