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
            Vector3 direction = Player.Instance.transform.position - _bossEnemy.transform.position;
            if (direction != Vector3.zero)
            {
                direction.Normalize();
            }
            else
            {
                direction = Vector3.forward; // Default fallback direction, change if needed
            }

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _bossEnemy.transform.rotation = Quaternion.Slerp(_bossEnemy.transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Calculate intended movement
            Vector3 nextPosition = Vector3.Lerp(startPosition, _targetPosition, elapsedTime / TIME);
            Vector3 movementDelta = nextPosition - _bossEnemy.transform.position;

            // Check for collision along the movement path
            RaycastHit hitInfo;
            bool hit = Physics.BoxCast(
                _bossEnemy.transform.position,
                _bossEnemy.transform.localScale * 1.5f / 2, // Adjust size to fit enemy's collider
                movementDelta.normalized,
                out hitInfo,
                Quaternion.identity,
                movementDelta.magnitude,
                _bossEnemy.collisionLayerMask);

            if (hit)
            {
                Debug.Log("Hit detected");
                // If a hit is detected, move only to the collision point
                //_bossEnemy.transform.position = hitInfo.point - movementDelta.normalized; // Offset to avoid overlap
                EnemyStateMachine.ChangeState(_nextState);
                return;
            }
            else
            {
                // If no collision, move normally
                _bossEnemy.transform.position = nextPosition;
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