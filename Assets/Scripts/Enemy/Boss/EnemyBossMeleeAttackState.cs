using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMeleeAttackState : EnemyState
{
    private int attackCount = 0;
    private bool isAttacking = true;
    private float attackSpeed = 2.5f;
    public EnemyBossMeleeAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss _bossEnemy;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        attackCount++;
        isAttacking = true;
        Debug.Log("Attack Count: " + attackCount);
    }
    public override void Update()
    {
        base.Update();

        if (isAttacking)
        {
            //move y-axis to .05 gradually
            _bossEnemy.meleePrefab.transform.localPosition = Vector3.MoveTowards(_bossEnemy.meleePrefab.transform.localPosition,
                new Vector3(_bossEnemy.meleePrefab.transform.localPosition.x, .05f, _bossEnemy.meleePrefab.transform.localPosition.z),
                Time.deltaTime * attackSpeed);
            if (Mathf.Approximately(_bossEnemy.meleePrefab.transform.localPosition.y, .05f))
            {
                Collider[] hitColliders = Physics.OverlapBox(_bossEnemy.meleePrefab.position,
                    _bossEnemy.meleePrefab.localScale / 2, _bossEnemy.meleePrefab.rotation);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player"))
                    {
                        IDamageable.Damage damage = new IDamageable.Damage(100, ElementType.Physical, _bossEnemy.transform);
                        Player.Instance.TakeDamage(damage);
                    }
                }

                isAttacking = false;
            }
        }
        else
        {
            // move y-axis to .95 gradually
            _bossEnemy.meleePrefab.transform.localPosition = Vector3.MoveTowards(_bossEnemy.meleePrefab.transform.localPosition,
                new Vector3(_bossEnemy.meleePrefab.transform.localPosition.x, .95f, _bossEnemy.meleePrefab.transform.localPosition.z),
                Time.deltaTime * attackSpeed);
            if (Mathf.Approximately(_bossEnemy.meleePrefab.transform.localPosition.y, .95f))
            {
                if (attackCount > 3)
                {
                    attackCount = 0;
                    EnemyStateMachine.ChangeState(_bossEnemy?.LaserState);
                }
                else
                {
                    EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
                }
            }
        }
    }
    public override void Exit()
    {
        base.Exit();

    }
}