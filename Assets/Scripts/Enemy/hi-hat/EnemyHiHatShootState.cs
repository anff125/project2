using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHiHatShootState : EnemyState
{
    private int _bulletsToShoot;
    private float _shootingTimer = 0f; 
    private int _currentBulletIndex = 0;
    List<float> timeSpace; 
    private bool CanShoot => _shootingTimer <= 0;
    private readonly EnemyHiHat _enemyElementalBoss;
 
    public EnemyHiHatShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _enemyElementalBoss = EnemyStateMachine.Enemy as EnemyHiHat;
        timeSpace = _enemyElementalBoss?.timeSpace;
    }

    public override void Enter()
    {
        base.Enter();
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
            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab,
                EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f,
                Quaternion.identity);
            // Set bullet properties
            bullet.GetComponent<Bullet>().SetBulletProperty(direction, 5, 10f);
            _enemyElementalBoss.RegisterBullet(bullet.GetComponent<Bullet>()); 
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