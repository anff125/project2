using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgunTrackPlayerState : EnemyState
{
    public EnemyShotgunTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(2f);
    }
    public override void Update()
    {
        base.Update();
        // Track player until player is in range 4 units (x and z axes only)
        Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;

        if (Vector3.Distance(enemyPosition, playerPosition) <= 4)
        {
            // Cast to EnemyShotgun
            EnemyShotgun shotgunEnemy = EnemyStateMachine.Enemy as EnemyShotgun;
            if (shotgunEnemy != null && CanExit)
            {
                EnemyStateMachine.ChangeState(shotgunEnemy.ShootState);
            }
        }
        else
        {
// Move towards player and rotate towards player
            EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, EnemyStateMachine.Enemy.speed * Time.deltaTime);
            EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}