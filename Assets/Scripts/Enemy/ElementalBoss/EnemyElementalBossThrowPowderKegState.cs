using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossThrowPowderKegState : EnemyState
{
    EnemyElementalBoss _bossEnemy;

    public EnemyElementalBossThrowPowderKegState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
    }

    public override void Enter()
    {
        base.Enter();
        _bossEnemy.animator.SetBool("isSpecialAttack", true);

    }

    public override void Update()
    {
        //instantiates a powder keg prefab at the boss's position and push it towards the player
        Transform powderKeg = Object.Instantiate(_bossEnemy.powderKegPrefab, _bossEnemy.transform.position, Quaternion.identity);

        powderKeg.GetComponent<Rigidbody>().AddForce((Player.Instance.GetPlayerPositionOnPlane() - _bossEnemy.transform.position).normalized * 5f,
            ForceMode.Impulse);
        powderKeg.GetComponent<PowderKeg>().Ignite(1f);
        EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseBaseState);
    }

    public override void Exit()
    {
        base.Exit();
        _bossEnemy.animator.SetBool("isSpecialAttack", false);


    }
}