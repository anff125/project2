using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossMoveToState : EnemyState
{
    public EnemyElementalBossMoveToState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    EnemyElementalBoss _bossEnemy;
    private Vector3 _targetPosition;
    private const float TIME = 1f;
    float elapsedTime;
    private EnemyState _nextState;

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(TIME);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyElementalBoss;
        elapsedTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        Vector3 startPosition = _bossEnemy.transform.position;
        if (elapsedTime < TIME)
        {
            // Rotate to face the player
            Vector3 direction = (Player.Instance.transform.position - _bossEnemy.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _bossEnemy.transform.rotation = Quaternion.Slerp(_bossEnemy.transform.rotation, lookRotation, Time.deltaTime * 5f);

            bool canMove = !Physics.BoxCast(_bossEnemy.transform.position, Vector3.one,
                (_targetPosition - startPosition), Quaternion.identity, _bossEnemy.moveDistance, _bossEnemy.collisionLayerMask);
            if (canMove)
            {
                _bossEnemy.transform.position = Vector3.Lerp(startPosition, _targetPosition, elapsedTime / TIME);
            }
            elapsedTime += Time.deltaTime;
        }
        else
        {
            EnemyStateMachine.ChangeState(_nextState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
    private void SetNextState(EnemyState nextState)
    {
        if (nextState == null)
        {
            _nextState = EnemyStateMachine.EnemyElementalBoss.TrackPlayerState;
        }
        else
        {
            _nextState = nextState;
        }
    }

    public void SetupMoveToState(Vector3 targetPosition, EnemyState nextState = null)
    {
        SetTargetPosition(targetPosition);
        SetNextState(nextState);
    }
}