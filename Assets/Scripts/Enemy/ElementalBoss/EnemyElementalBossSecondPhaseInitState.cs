using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossSecondPhaseInitState : EnemyState
{
    public EnemyElementalBossSecondPhaseInitState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    EnemyElementalBoss _bossEnemy;

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(InstantiateManager.Instance.GetLavaStageDuration());
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
        if (_bossEnemy != null)
        {
            _bossEnemy.isSecondPhase = true;
        }

        InstantiateManager.Instance.StartLavaBallStage();
    }

    public override void Update()
    {
        base.Update();
        var position = new Vector3(0, 0, 0);
        _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.SecondPhaseBaseState);
        EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
        // EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseBaseState);
    }

    public override void Exit()
    {
        base.Exit();
        GameManager.Instance.mainCamera.gameObject.SetActive(true);
    }
}