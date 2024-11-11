using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2Danmoku1State : EnemyState
{
    public EnemyBoss2Danmoku1State(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private BulletEmitter _bulletEmitter;
    private float _timer;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;

        _bossEnemy.bulletEmitters[0].PlayAnimationMultipleTimes(1, 2, out float clipLength0);
        _bossEnemy.bulletEmitters[1].PlayAnimationMultipleTimes(1, 2, out float clipLength1);
        _timer = clipLength0 + 1f;
    }

    public override void Update()
    {
        base.Update();

        if (_timer < 0){
            EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
        }

        _timer -= Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
