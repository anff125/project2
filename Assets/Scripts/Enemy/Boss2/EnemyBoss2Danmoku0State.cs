using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2Danmoku0State : EnemyState
{
    public EnemyBoss2Danmoku0State(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private BulletEmitter _bulletEmitter;
    private float _timer;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;

        _bossEnemy.bulletEmitters[0].PlayAnimationMultipleTimes(0, 3, out float clipLength);
        _timer = clipLength + 1f;
    }

    public override void Update()
    {
        base.Update();

        if (_timer < 0){
            EnemyStateMachine.ChangeState(_bossEnemy.Danmoku1State);
        }

        _timer -= Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
