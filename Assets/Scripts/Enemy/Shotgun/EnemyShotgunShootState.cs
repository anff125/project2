using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgunShootState : EnemyState
{
    public EnemyShotgunShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(.3f);
        // Activate the exclamation mark image 0.1 seconds before shooting
        EnemyStateMachine.Enemy.animator.SetBool("isAttack", true);
        EnemyStateMachine.Enemy.exclamationMark.gameObject.SetActive(true);
        EnemyStateMachine.Enemy.StartCoroutine(ShootWithDelay(0.3f));
    }

    private IEnumerator ShootWithDelay(float delay)
    {
        // Wait for the delay duration
        yield return new WaitForSeconds(delay);
        EnemyStateMachine.Enemy.exclamationMark.gameObject.SetActive(false);
        
        // Shoot 5 bullets in a Circular sector with 120 degrees in front of the enemy
        Vector3 direction = EnemyStateMachine.Enemy.transform.forward;

        for (int i = 0; i < 5; i++)
        {
            var bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab, EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f, Quaternion.identity);
            // Set bullet direction
            Vector3 bulletDirection = Quaternion.Euler(0, -30 + i * 15, 0) * direction;
            bullet.GetComponent<Bullet>().SetBulletProperty(bulletDirection, 10, 1f);
        }
        EnemyStateMachine.Enemy.animator.SetBool("isAttack", false);
    }

    public override void Update()
    {
        base.Update();
        EnemyShotgun shotgunEnemy = EnemyStateMachine.Enemy as EnemyShotgun;
        if (shotgunEnemy != null)
        {
            EnemyStateMachine.ChangeState(shotgunEnemy.TrackPlayerState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}