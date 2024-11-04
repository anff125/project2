using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElementalBossMeleeState : EnemyState
{
    public EnemyElementalBossMeleeState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private float shootAngle = 30;
    private float bulletCount = 7;
    private float delay = .3f;
    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(delay);

        EnemyStateMachine.EnemyElementalBoss.exclamationMark.gameObject.SetActive(true);
        EnemyStateMachine.EnemyElementalBoss.StartCoroutine(ShootWithDelay());
        EnemyStateMachine.EnemyElementalBoss.meleeCount++;
    }

    private IEnumerator ShootWithDelay()
    {
        // Wait for the delay duration
        float blinkInterval = .1f;
        float elapsed = 0f;
        while (elapsed <= delay)
        {
            EnemyStateMachine.EnemyElementalBoss.exclamationMark.gameObject.SetActive(!EnemyStateMachine.EnemyElementalBoss.exclamationMark.gameObject.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        EnemyStateMachine.EnemyElementalBoss.exclamationMark.gameObject.SetActive(false);

        // Shoot 5 bullets in a Circular sector with 120 degrees in front of the enemy
        Vector3 direction = EnemyStateMachine.Enemy.transform.forward;

        for (int i = 0; i < bulletCount; i++)
        {
            var bullet = Object.Instantiate(EnemyStateMachine.EnemyElementalBoss.bulletPrefab, EnemyStateMachine.EnemyElementalBoss.transform.position + Vector3.up * 0.3f, Quaternion.identity);
            // Set bullet direction
            Vector3 bulletDirection = Quaternion.Euler(0, (i * shootAngle / (bulletCount - 1)) - (shootAngle / 2), 0) * direction;
            bullet.GetComponent<Bullet>().SetBulletProperty(bulletDirection, 15, 2f);
        }
    }

    public override void Update()
    {
        base.Update();
        if (EnemyStateMachine.EnemyElementalBoss != null)
        {
            EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyElementalBoss.TrackPlayerState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}