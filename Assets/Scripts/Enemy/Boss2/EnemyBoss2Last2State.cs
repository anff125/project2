using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2Last2State : EnemyState
{
    public EnemyBoss2Last2State(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private const float BULLET_SPAWN_INTERVAL = 0.05f; // Time delay between bullet spawns
    private const float BULLET_SPEED = 30f; // Bullet movement speed
    private const int WALL_START_Z = 20;
    private const int WALL_END_Z = -20;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        SetStateChangeCooldown(1f);
        _bossEnemy.StartCoroutine(SpawnBulletsAlongWall());
        _bossEnemy.StartCoroutine(PlayAnimationRoutine());
    }
    private IEnumerator PlayAnimationRoutine()
    {
        float clipLength = 6f;
        SetStateChangeCooldown(clipLength);

        // Play the animation without delay
        EnemyStateMachine.EnemyBoss2.bulletEmitters[0].PlayAnimationMultipleTimes(7, 6, out float len);

        // Wait for the exact duration of the animation before starting the next iteration
        yield return new WaitForSeconds(clipLength);
    }
    private IEnumerator SpawnBulletsAlongWall()
    {
        for (int i = 0; i < 4; ++i)
        {
            for (int z = WALL_START_Z; z >= WALL_END_Z; z--)
            {
                Vector3 spawnPosition1 = new Vector3(20, 0, z);
                Vector3 spawnPosition2 = new Vector3(-20, 0, -z);
                Vector3 spawnPosition3 = new Vector3(-z, 0, 20);
                Vector3 spawnPosition4 = new Vector3(z, 0, -20);
                Vector3 direction1 = spawnPosition2.normalized;
                Vector3 direction2 = spawnPosition1.normalized;
                Vector3 direction3 = spawnPosition4.normalized;
                Vector3 direction4 = spawnPosition3.normalized;

                // Instantiate the bullet
                Transform bullet1 = Object.Instantiate(_bossEnemy.straightBulletPrefab, spawnPosition1, Quaternion.identity);
                Transform bullet2 = Object.Instantiate(_bossEnemy.straightBulletPrefab, spawnPosition2, Quaternion.identity);
                Transform bullet3 = Object.Instantiate(_bossEnemy.straightBulletPrefab, spawnPosition3, Quaternion.identity);
                Transform bullet4 = Object.Instantiate(_bossEnemy.straightBulletPrefab, spawnPosition4, Quaternion.identity);

                // Assign its movement towards the origin
                bullet1.GetComponent<BulletNew>().SetBulletProperty(spawnPosition1, spawnPosition1, Quaternion.LookRotation(direction1),
                        direction1, BULLET_SPEED, 5, 270f, 0f, 0f); // Bullets move left
                bullet2.GetComponent<BulletNew>().SetBulletProperty(spawnPosition2, spawnPosition2, Quaternion.LookRotation(direction2),
                        direction2, BULLET_SPEED, 5, 270f, 0f, 0f); // Bullets move right
                bullet3.GetComponent<BulletNew>().SetBulletProperty(spawnPosition3, spawnPosition3, Quaternion.LookRotation(direction3),
                        direction3, BULLET_SPEED, 5, 270f, 0f, 0f); // Bullets move left
                bullet4.GetComponent<BulletNew>().SetBulletProperty(spawnPosition4, spawnPosition4, Quaternion.LookRotation(direction4),
                        direction4, BULLET_SPEED, 5, 270f, 0f, 0f); // Bullets move right

                // Wait before spawning the next bullet
                yield return new WaitForSeconds(BULLET_SPAWN_INTERVAL);
            }
        }
        // After all bullets are spawned, return to another state
        EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseTrackPlayerState);
    }    
}
