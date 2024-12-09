using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFortShootState : EnemyState
{
    
    private EnemyFort fortEnemy;
    private int _bulletsPerShoot = 3;
    private float _shootingTimer = 0f; // Cooldown timer
    private readonly float _shootingInterval; // Interval between shots
    private bool CanShoot => _shootingTimer <= 0;
    private float _selfHurtTimer = 0;

    // Add the shooting interval as a parameter in the constructor
    public EnemyFortShootState(EnemyStateMachine enemyStateMachine, float shootingInterval = 1.5f) : base(enemyStateMachine)
    {
        _shootingInterval = shootingInterval;
    }

    public override void Enter()
    {
        base.Enter();
        fortEnemy = EnemyStateMachine.Enemy as EnemyFort;
    }

    public override void Update()
    {
        base.Update();

        if (fortEnemy.IsActivated == false)
        {
            return;
        }

        Vector3 selfPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        Enemy enemy = DetectAndChooseEnemy(fortEnemy);
        bool findTarget = false;

        if (fortEnemy.IsAlliedWithPlayer)
        {
            _selfHurtTimer += Time.deltaTime;
            if (_selfHurtTimer > 1f)
            {
                _selfHurtTimer = 0f;
                float damageAmount = fortEnemy.maxHealth / 15;
                IDamageable.Damage damage = new IDamageable.Damage(damageAmount, ElementType.Physical, fortEnemy.transform);
                fortEnemy.TakeDamage(damage);
            }
        }

        if (fortEnemy.IsAlliedWithPlayer && enemy != null)
        {
            findTarget = true;
            Vector3 enemyPosition = enemy.transform.position;
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(enemyPosition - selfPosition);
        }
        else if (!fortEnemy.IsAlliedWithPlayer)
        {
            findTarget = true;
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - selfPosition);
        }
        

        // Handle the cooldown timer
        if (_shootingTimer > 0)
        {
            _shootingTimer -= Time.deltaTime;
        }

        // Shoot if the timer has reached 0 (CanShoot) and reset the timer
        if (CanShoot && findTarget)
        {
            LayerMask bulletLayerMask = fortEnemy.IsAlliedWithPlayer ? fortEnemy.playerLayerMask : fortEnemy.enemyLayerMask;
            if (fortEnemy.IsAlliedWithPlayer)
                Debug.Log("IsAlliedWithPlayer == true");
            else
                Debug.Log("IsAlliedWithPlayer == false");

            Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
            for (int i = 0; i < _bulletsPerShoot; i++)
            {
                Vector3 bulletDirection = Quaternion.Euler(0, -10f + i * 10f, 0) * direction;
                var bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab, 
                    EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f, Quaternion.LookRotation(bulletDirection));
                
                Bullet tempBullet = bullet.GetComponent<Bullet>();
                if (fortEnemy.IsAlliedWithPlayer)
                {
                    tempBullet.SetTextureForPlayer();
                }
                tempBullet.SetBulletProperty(bulletDirection, 10, 5f, 10);
                tempBullet.SetShooterLayerMask(bulletLayerMask);
            }

            _shootingTimer = _shootingInterval; // Reset the timer to the shooting interval
        }
    }

    private Enemy DetectAndChooseEnemy(EnemyFort fortEnemy)
    {
        foreach (var enemy in fortEnemy.trackEnemies)
        {
            // Debug.Log(enemy.name + "in the list");
            if (enemy != null && enemy != fortEnemy && enemy.IsAlliedWithPlayer == false)
            {
                return enemy;
            }
        }
        return null;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
