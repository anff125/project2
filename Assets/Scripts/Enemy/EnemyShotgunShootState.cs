using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgunShootState : EnemyState
{
    public EnemyShotgunShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy Shotgun Shoot Enter");
        //Shoot 5 bullets in a Circular sector with 120 degrees in front of the enemy
        Vector3 direction = EnemyStateMachine.Enemy.transform.forward;
        for (int i = 0; i < 5; i++)
        {
            Transform bullet = Object.Instantiate(EnemyStateMachine.Enemy.bulletPrefab, EnemyStateMachine.Enemy.transform.position + Vector3.up * 0.3f, Quaternion.identity);
            // Set bullet direction
            Vector3 bulletDirection = Quaternion.Euler(0, -30 + i * 15, 0) * direction;
            bullet.GetComponent<Bullet>().SetBulletProperty(bulletDirection, 20, .5f);
        }
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