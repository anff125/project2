using System.Collections;
using UnityEngine;

public class EnemyLaserTrackPlayerState : EnemyState
{
    private float timeRunningAwayFromPlayer;

    public EnemyLaserTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(2f);
        timeRunningAwayFromPlayer = 0f;
    }

    public override void Update()
    {
        base.Update();
        timeRunningAwayFromPlayer += Time.deltaTime;

        Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;

        float distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);

        if (distanceToPlayer <= 6)
        {
            // 遠離玩家，並持續追蹤玩家
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, -EnemyStateMachine.Enemy.speed * Time.deltaTime * 0.3f);
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);

            // 若時間超過1秒，切換到雷射射擊狀態
            if (timeRunningAwayFromPlayer > 1f)
            {
                var laserEnemy = EnemyStateMachine.Enemy as EnemyLaser;
                if (laserEnemy != null)
                {
                    timeRunningAwayFromPlayer = 0f;
                    EnemyStateMachine.ChangeState(laserEnemy.ShootState);
                }
            }
        }
        else if (distanceToPlayer > 6 && distanceToPlayer <= 8)
        {
            // 在中距離保持與玩家的距離，並切換至射擊狀態
            var laserEnemy = EnemyStateMachine.Enemy as EnemyLaser;
            if (laserEnemy != null)
            {
                EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
                EnemyStateMachine.ChangeState(laserEnemy.ShootState);
            }
        }
        else
        {
            // 靠近玩家
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, EnemyStateMachine.Enemy.speed * Time.deltaTime);
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
