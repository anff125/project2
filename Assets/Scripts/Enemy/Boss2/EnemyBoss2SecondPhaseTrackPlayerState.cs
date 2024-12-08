using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2SecondPhaseTrackPlayerState : EnemyState
{
    public EnemyBoss2SecondPhaseTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private float timeTrackingPlayer;
    private float circleAngle = 0f;
    private bool isRetreating = false;
    private float retreatDuration = 2f;
    private float retreatTimer = 0f;
    private float damageThreshold = 50f;  // Customize this value based on boss behavior
    private float accumulatedDamage = 0f;
    private int changeStateCounter = 0;
    private WeightedState randomState;

    public override void Enter()
    {
        base.Enter();
        float cooldown = Random.Range(.25f, .5f);
        SetStateChangeCooldown(cooldown);
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        timeTrackingPlayer = 0f;
        circleAngle = 0f;
        accumulatedDamage = 0f;
        isRetreating = false;
        retreatTimer = 0f;
        changeStateCounter += 1;

        Debug.Log("changeStateCounter: " + changeStateCounter);

        if (changeStateCounter > 4)
        {
            changeStateCounter = 0;
            ResetWeightedStates();
        }

        randomState = GetRandomWeightedState();
    }

    public override void Update()
    {
        base.Update();
        timeTrackingPlayer += Time.deltaTime;

        Vector3 enemyPosition = EnemyStateMachine.Enemy.transform.position;
        Vector3 playerPosition = Player.Instance.transform.position;
        enemyPosition.y = 0;
        playerPosition.y = 0;
        // var dis = Vector3.Distance(enemyPosition, playerPosition);
        // float randomNumber = Random.Range(0f, 1f);
        // float reverseOfNormalStates = 1f / 3f;

        // Check if the boss should retreat after taking enough damage
        // if (accumulatedDamage >= damageThreshold)
        // {
        //     isRetreating = true;
        //     accumulatedDamage = 0f;  // Reset damage taken
        // }

        // if (isRetreating)
        // {
        //     RetreatFromPlayer(playerPosition);
        //     return;
        // }

        // Circle around at a distance of 4 units
        CircleAroundPlayer(playerPosition, 4f);  
        

        if (randomState != null)
        {
            var position = _bossEnemy.GetFixedDistancePositionAroundPlayer(7f);
            if (randomState.State == _bossEnemy.Danmoku5State)
            {
                position = new Vector3(0, 0, 0);
            }
            _bossEnemy.MoveToState.SetupMoveToState(position, randomState.State);
            EnemyStateMachine.ChangeState(_bossEnemy.MoveToState);
        }
    }

    private WeightedState GetRandomWeightedState()
    {
        float totalWeight = 0f;

        // Calculate total weight
        foreach (var ws in _bossEnemy.weightedStatesPhase2)
        {
            totalWeight += ws.Weight;
        }

        // Generate a random value
        float randomValue = Random.Range(0, totalWeight);

        // Iterate through the list and select the corresponding state
        foreach (var ws in _bossEnemy.weightedStatesPhase2)
        {
            if (ws.Weight > 0 && randomValue <= ws.Weight)
            {
                Debug.Log("total weight: " + totalWeight + " state.Weight = " + ws.Weight);
                ws.Weight = ws.Weight - 4f > 0 ? ws.Weight - 4f : 0f;
                return ws;
            }
            randomValue -= ws.Weight;
        }

        // Fallback (should never reach here if weights are set properly)
        return null;
    }
    private void ResetWeightedStates()
    {
        foreach (var ws in _bossEnemy.weightedStatesPhase2)
        {
            ws.ResetWeight();
        }
    }
    
    private void CircleAroundPlayer(Vector3 playerPosition, float radius)
    {
        circleAngle += Time.deltaTime;  // Adjust the angle over time to move around the player
        float x = Mathf.Cos(circleAngle) * radius;
        float z = Mathf.Sin(circleAngle) * radius;
        Vector3 targetPosition = playerPosition + new Vector3(x, 0, z);
        Vector3 direction = targetPosition - _bossEnemy.transform.position;
        direction.y = 0f;


        // Perform the BoxCast to check if there's an obstacle between the boss and the target position
        if (!Physics.BoxCast(_bossEnemy.transform.position, Vector3.one,
                        direction, _bossEnemy.transform.rotation, _bossEnemy.moveDistance, _bossEnemy.collisionLayerMask))
        {
            // Move towards the target position if no obstacle is detected
            _bossEnemy.transform.position = Vector3.MoveTowards(
                _bossEnemy.transform.position,
                targetPosition,
                EnemyStateMachine.Enemy.speed * Time.deltaTime
            );

            // Orient the boss to face the player
            _bossEnemy.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void RetreatFromPlayer(Vector3 playerPosition)
    {
        retreatTimer += Time.deltaTime;
        if (retreatTimer >= retreatDuration)
        {
            // End retreat
            isRetreating = false;
            retreatTimer = 0f;
            return;
        }

        // Move in the opposite direction from the player
        Vector3 directionAwayFromPlayer = (_bossEnemy.transform.position - playerPosition).normalized;
        Vector3 retreatPosition = _bossEnemy.transform.position + directionAwayFromPlayer * 5f; // Adjust distance as needed
        _bossEnemy.transform.position = Vector3.MoveTowards(
            _bossEnemy.transform.position,
            retreatPosition,
            EnemyStateMachine.Enemy.speed * Time.deltaTime
        );
    }
    public override void Exit()
    {
        base.Exit();
    }
}
