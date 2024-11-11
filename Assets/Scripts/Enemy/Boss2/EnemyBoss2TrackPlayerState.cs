using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2TrackPlayerState : EnemyState
{
    public EnemyBoss2TrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private float timeTrackingPlayer;

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(.5f);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        timeTrackingPlayer = 0f;
    }

    public override void Update()
    {
        base.Update();
        timeTrackingPlayer += Time.deltaTime;

        // Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        // Vector3 playerPosition = Player.Instance.transform.position;
        // enemyPosition.y = 0;
        // playerPosition.y = 0;
        // var dis = Vector3.Distance(enemyPosition, playerPosition);


        if (timeTrackingPlayer > 1f || (_bossEnemy.danmokuCount0 > 1 && _bossEnemy.danmokuCount2 >= 2))
        {
            var position = _bossEnemy.GetFixedDistancePositionAroundPlayer(10f);
            _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.Danmoku1State);
            EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
            // EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
            // EnemyStateMachine.ChangeState(_bossEnemy.Danmoku1State);
        }
        else if (_bossEnemy.danmokuCount2 <= _bossEnemy.danmokuCount0)
        {
            var position = _bossEnemy.GetFixedDistancePositionAroundPlayer(7f);
            _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.Danmoku2State);
            EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
            // EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, EnemyStateMachine.Enemy.speed * Time.deltaTime);
            // EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
        }
        else
        {
            var position = _bossEnemy.GetFixedDistancePositionAroundPlayer(5f);
            _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.Danmoku0State);
            EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
