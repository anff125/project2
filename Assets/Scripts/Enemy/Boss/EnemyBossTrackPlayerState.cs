using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossTrackPlayerState : EnemyState
{
    public EnemyBossTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    EnemyBoss _bossEnemy;

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(2f);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        Debug.Log("Boss Track Player State");
    }
    public override void Update()
    {
        base.Update();
        if (_bossEnemy.currentHealth < _bossEnemy.maxHealth / 2)
            EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseIdleState);
        Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;
        var dis = Vector3.Distance(enemyPosition, playerPosition);

        if (dis < 5f)
        {
            EnemyStateMachine.ChangeState(_bossEnemy.LaserState);
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