using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2SpawnFortState : EnemyState
{
    private EnemyBoss2 _bossEnemy;
    private float spawnRange = 8f;
    private int numberOfEnemies = 2;
    private float throwDuration = 1f; // Duration for the throw
    private Vector3[] targetPositions;

    public EnemyBoss2SpawnFortState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;

        // Define target positions for the spawned enemies
        targetPositions = new Vector3[numberOfEnemies];
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float offsetX = Random.Range(-spawnRange, spawnRange);
            float offsetZ = Random.Range(-spawnRange, spawnRange);
            targetPositions[i] = _bossEnemy.transform.position + new Vector3(offsetX, 0, offsetZ);
        }

        // Start the throwing process
        _bossEnemy.StartCoroutine(ThrowEnemiesRoutine());
    }

    private IEnumerator ThrowEnemiesRoutine()
    {
        Transform[] spawnedEnemies = new Transform[numberOfEnemies];

        // Spawn enemies at the boss's position
        for (int i = 0; i < numberOfEnemies; i++)
        {
            spawnedEnemies[i] = Object.Instantiate(
                _bossEnemy.fortEnemyPrefab, 
                _bossEnemy.transform.position + Vector3.up, 
                Quaternion.identity
            );
        }

        float elapsedTime = 0f;

        // Animate the throw
        while (elapsedTime < throwDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / throwDuration;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                if (spawnedEnemies[i] != null)
                {
                    Vector3 start = _bossEnemy.transform.position + Vector3.up;
                    Vector3 target = targetPositions[i];
                    float height = 5f; // Peak height of the arc

                    // Calculate parabolic position
                    float yOffset = height * (1 - 4 * Mathf.Pow(normalizedTime - 0.5f, 2));
                    Vector3 currentPosition = Vector3.Lerp(start, target, normalizedTime);
                    currentPosition.y += yOffset;

                    spawnedEnemies[i].transform.position = currentPosition;
                }
            }

            yield return null;
        }

        // Ensure enemies land exactly at the target positions
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (spawnedEnemies[i] != null)
            {
                spawnedEnemies[i].transform.position = targetPositions[i];
                // Activate the enemy logic
                spawnedEnemies[i].GetComponent<EnemyFort>().Activate();
            }
        }

        // Transition to the next state
        if (EnemyStateMachine.EnemyBoss2 != null){
            if (EnemyStateMachine.EnemyBoss2.InSecondPhase)
            {
                EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.SecondPhaseTrackPlayerState);
            }
            else
            {
                EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.TrackPlayerState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
