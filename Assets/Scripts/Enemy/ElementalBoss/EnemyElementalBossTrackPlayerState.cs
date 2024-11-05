using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossTrackPlayerState : EnemyState
{
    public EnemyElementalBossTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    EnemyElementalBoss _bossEnemy;
    private float timeTrackingPlayer;
    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(2f);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
        timeTrackingPlayer = 0f;
    }

    public override void Update()
    {
        base.Update();
        timeTrackingPlayer += Time.deltaTime;
        Vector3 enemyPosition = _bossEnemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;
        var dis = Vector3.Distance(enemyPosition, playerPosition);

        if (timeTrackingPlayer > 10f || _bossEnemy.meleeCount > 2)
        {
            EnemyStateMachine.ChangeState(_bossEnemy.ShootState);
        }
        else if (dis < 5f)
        {
            EnemyStateMachine.ChangeState(_bossEnemy.MeleeState);
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