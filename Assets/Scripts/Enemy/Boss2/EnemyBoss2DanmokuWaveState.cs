using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2DanmokuWaveState : EnemyState
{
    public EnemyBoss2DanmokuWaveState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private float delay = .3f;
    private const float endingWaitSeconds = 1f;
    private const float waveSpawnInterval = 0.25f; // Time between starting new waves
    private const float bulletSpawnRate = 0.2f; // Time between bullet spawns within a wave
    private const float waveDuration = 3f; // Duration for which each wave spawns bullets
    private const float waveOffsetZ = 5f; // Offset between each wave's Z position
    private const float startZ = 25f; // Starting Z position
    private const float minZ = -25f; // Minimum Z position before stopping



    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        _bossEnemy.StartCoroutine(SpawnWaves());
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator SpawnWaves()
    {
        float blinkInterval = .1f;
        float elapsed = 0f;
        while (elapsed <= delay)
        {
            EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.SetActive(!EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.SetActive(false);

        float currentZ = startZ;

        while (currentZ >= minZ)
        {
            // Start a new wave
            _bossEnemy.StartCoroutine(SpawnBulletsAtPosition(new Vector3(25, 0, currentZ), new Vector3(-1, 0, 0)));
            _bossEnemy.StartCoroutine(SpawnBulletsAtPosition(new Vector3(-currentZ, 0, -25), new Vector3(0, 0, 1)));

            // Update position for the next wave
            currentZ -= waveOffsetZ;

            // Wait before spawning the next wave
            yield return new WaitForSeconds(waveSpawnInterval);
        }
        yield return new WaitForSeconds(endingWaitSeconds);

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

    private IEnumerator SpawnBulletsAtPosition(Vector3 position, Vector3 direction)
    {
        float elapsedTime = 0f;

        while (elapsedTime < waveDuration)
        {
            // Spawn a bullet
            SpawnBullet(position, direction);

            // Wait for the next bullet
            yield return new WaitForSeconds(bulletSpawnRate);

            elapsedTime += bulletSpawnRate;
        }
    }

    private void SpawnBullet(Vector3 position, Vector3 direction)
    {
        Transform bullet = Object.Instantiate(_bossEnemy.waveBulletPrefab, position, Quaternion.identity);
        bullet.GetComponent<BulletNew>().SetBulletProperty(position, position, Quaternion.LookRotation(direction),
                    direction, 10, 6, 270f, 0f, 0f); // Bullets move left
    }
}
