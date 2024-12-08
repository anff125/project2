using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireTrackPlayerState : EnemyState
{
    public EnemyFireTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private float timeRunningAwayFromPlayer;

    [SerializeField, Range(0.1f, 20f)] private float rotationSpeed = 0.6f; // 每秒最大旋轉角度

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

        Vector3 direction = (playerPosition - enemyPosition).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        EnemyStateMachine.Enemy.transform.rotation = Quaternion.Slerp(
            EnemyStateMachine.Enemy.transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );

        if (dis <= 6)
        {
            //move away from player
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, -EnemyStateMachine.Enemy.speed * Time.deltaTime * .3f);
            // EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);

            // Change to shoot state if this lasts longer than 1f
            if (timeRunningAwayFromPlayer > 1f)
            {
                EnemyFire fireEnemy = EnemyStateMachine.Enemy as EnemyFire;
                if (fireEnemy != null)
                {
                    timeRunningAwayFromPlayer = 0f;
                    EnemyStateMachine.ChangeState(fireEnemy.ShootState);
                }
            }
        }
        else if (dis > 6 && dis <= 8)
        {
            // Cast to EnemyFire
            EnemyFire fireEnemy = EnemyStateMachine.Enemy as EnemyFire;
            if (fireEnemy != null)
            {
                // EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
                EnemyStateMachine.ChangeState(fireEnemy.ShootState);
            }
        }
        else
        {
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, EnemyStateMachine.Enemy.speed * Time.deltaTime);
            // EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}