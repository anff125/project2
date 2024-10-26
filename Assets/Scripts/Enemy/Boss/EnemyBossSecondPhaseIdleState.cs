using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSecondPhaseIdleState : EnemyState
{
    public EnemyBossSecondPhaseIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    EnemyBoss _bossEnemy;
    private int _shootCount = 0;
    private int _smashCount = 0;
    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        if (_bossEnemy == null) return;
        _bossEnemy.meleePrefab.gameObject.SetActive(false);
        _bossEnemy.smashPrefab.gameObject.SetActive(false);
        _bossEnemy.laserPrefab.gameObject.SetActive(false);
        SetStateChangeCooldown(1f);
    }
    public override void Update()
    {
        base.Update();
        if (_shootCount < 3)
        {
            if (EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseLaserState))
            {
                _shootCount++;
                if (_shootCount == 3)
                {
                    _smashCount = 0;
                }
            }
        }
        else if (_smashCount < 3)
        {
            if (EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseSmashState))
            {
                _smashCount++;
                if (_smashCount == 3)
                {
                    _shootCount = 0;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}