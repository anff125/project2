using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifleTrackPlayerState : EnemyState
{
    public EnemyRifleTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private float timeRunningAwayFromPlayer;
    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(2f);
    }
    public override void Update()
    {
        base.Update();
        timeRunningAwayFromPlayer += Time.deltaTime;
        Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;
        var dis = Vector3.Distance(enemyPosition, playerPosition);
        if (dis <= 6)
        {
            //move away from player
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, -EnemyStateMachine.Enemy.speed * Time.deltaTime * .3f);
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);

            // Change to shoot state if this lasts longer than 1f
            if (timeRunningAwayFromPlayer > 1f)
            {
                EnemyRifle rifleEnemy = EnemyStateMachine.Enemy as EnemyRifle;
                if (rifleEnemy != null)
                {
                    timeRunningAwayFromPlayer = 0f;
                    EnemyStateMachine.ChangeState(rifleEnemy.ShootState);
                }
            }
        }
        else if (dis > 6 && dis <= 8)
        {
            // Cast to EnemyRifle
            EnemyRifle rifleEnemy = EnemyStateMachine.Enemy as EnemyRifle;
            if (rifleEnemy != null)
            {
                EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
                EnemyStateMachine.ChangeState(rifleEnemy.ShootState);
            }
        }
        else
        {
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, EnemyStateMachine.Enemy.speed * Time.deltaTime);
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}