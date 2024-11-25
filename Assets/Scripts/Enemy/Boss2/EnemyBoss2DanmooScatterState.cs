using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2DanmooScatterState : EnemyState
{
    public EnemyBoss2DanmooScatterState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private float _waitTimer;
    private int _projectileNumber = 40;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        _waitTimer = 2.5f;

        // Instantiate bullets
        Vector3 direction = _bossEnemy.transform.forward;
        Vector3 center = _bossEnemy.transform.position;
        Quaternion rotation = _bossEnemy.transform.rotation;
        // for (int i = 0; i < _projectileNumber; i++)
        // {
        //     Transform bullet = Object.Instantiate(_bossEnemy.scatterBulletPrefab, center, rotation);
        //     // Set bullet direction
        // }
    }

    public override void Update()
    {
        base.Update();
        
        if (_waitTimer <= 0)
        {
            EnemyStateMachine.ChangeState(_bossEnemy.Danmoku1State);
        }

        _waitTimer -= Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
