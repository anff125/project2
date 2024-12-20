using UnityEngine;

public class EnemyElementalBossTrackPlayerState : EnemyState
{
    public EnemyElementalBossTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    EnemyElementalBoss _bossEnemy;
    private float timeTrackingPlayer;
    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(.5f);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
        timeTrackingPlayer = 0f;
        if (_bossEnemy != null && _bossEnemy.isSecondPhase)
        {
            Debug.LogError("Boss is in second phase and should not be in track player state");
        }
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

        if (_bossEnemy.currentHealth <= _bossEnemy.maxHealth / 2)
        {
            var position = new Vector3(0, 5, 0);
            _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.SecondPhaseInitState);
            EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
            // EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseInitState);
        }
        else if (timeTrackingPlayer > 10f || _bossEnemy.meleeCount > 1)
        {
            var position = _bossEnemy.GetFixedDistancePositionAroundPlayer(10f);
            position.y = 0;
            //randomly choose between shoot and IceBlockLaser
            if (Random.Range(0, 2) == 0)
            {
                _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.IceBlockLaserState);
                EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
            }
            else
            {
                _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.ShootState);
                EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
            }
            // _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.IceBlockLaserState);
            // EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
            //EnemyStateMachine.ChangeState(_bossEnemy.ShootState);
        }
        // else if (dis < 5f)
        // {
        //     EnemyStateMachine.ChangeState(_bossEnemy.MeleeState);
        // }
        else
        {
            // EnemyStateMachine.Enemy.transform.position = Vector3.MoveTowards(enemyPosition, playerPosition, EnemyStateMachine.Enemy.speed * Time.deltaTime);
            // EnemyStateMachine.Enemy.transform.rotation = Quaternion.LookRotation(playerPosition - enemyPosition);
            var position = _bossEnemy.GetFixedDistancePositionAroundPlayer(4.7f);
            position.y = 0;
            _bossEnemy.MoveToState.SetupMoveToState(position, _bossEnemy.MeleeState);
            EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}