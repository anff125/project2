using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2DanmooCircleState : EnemyState
{
    public EnemyBoss2DanmooCircleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private int _waveCount;
    private float _waveTimer;
    private float _waveCooldown = 1f;
    private int _projectileNumber = 12;

    public override void Enter()
    {
        base.Enter();
        _waveCount = 3;
        _waveTimer = 0;
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
    }

    public override void Update()
    {
        base.Update();

        // if (_waveTimer > 0)
        // {
        //     _waveTimer -= Time.deltaTime;
        // }
        // else if (_waveCount > 0)
        // {
        //     // Instantiate bullets
        //     Vector3 center = _bossEnemy.transform.position;
        //     Quaternion rotation = _bossEnemy.transform.rotation;
        //     for (int i = 0; i < _projectileNumber; i++)
        //     {
        //         Transform bullet = Object.Instantiate(_bossEnemy.bulletPrefab, center, rotation);
        //         // Set bullet property
        //         float angle = i * (360f / _projectileNumber);
        //         bullet.GetComponent<BulletCircle>().SetCircleProperty(center, angle);
        //     }

        //     Debug.Log("wave " + _waveCount);
        //     _waveCount--;
        //     _waveTimer = _waveCooldown;
        // }
        // else
        // {
        //     EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
