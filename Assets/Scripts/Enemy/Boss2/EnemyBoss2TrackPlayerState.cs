using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2TrackPlayerState : EnemyState
{
    public EnemyBoss2TrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(.1f);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
    }

    public override void Update()
    {
        base.Update();

        Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;
        var dis = Vector3.Distance(enemyPosition, playerPosition);

        if (dis < 5f)
        {
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
            EnemyStateMachine.ChangeState(_bossEnemy.CircleState);
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
