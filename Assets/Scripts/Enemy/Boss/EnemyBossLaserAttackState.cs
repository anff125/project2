using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossLaserAttackState : EnemyState
{
    private EnemyBoss _bossEnemy;
    public EnemyBossLaserAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private const float ROTATION_SPEED = 20f; // degrees per second
    float _rotationAmount = 0;
    float _laserScale = 0;
    Collider laserPrefabCollider;
    public override void Enter()
    {
        base.Enter();
        //flash to 0,0,0
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        EnemyStateMachine.Enemy.transform.position = new Vector3(0, 0, 0);
        _rotationAmount = 0;
        _laserScale = 0;
        _bossEnemy?.laserPrefab.gameObject.SetActive(true);
        laserPrefabCollider = _bossEnemy.laserPrefab.GetComponent<CapsuleCollider>();
    }
    public override void Update()
    {
        base.Update();
        //show laser and rotate 360 degrees around y-axis then change state to track player
        _rotationAmount += ROTATION_SPEED * Time.deltaTime;
        _laserScale += Time.deltaTime;
        _bossEnemy.laserPrefab.transform.rotation = Quaternion.Euler(90, _rotationAmount, 0);
        //make y scale to 10
        _bossEnemy.laserPrefab.localScale = new Vector3(_bossEnemy.laserPrefab.localScale.x, _laserScale, _bossEnemy.laserPrefab.localScale.z);
        if (_rotationAmount >= 360)
        {
            EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
        }
        if (laserPrefabCollider.bounds.Contains(Player.Instance.transform.position))
        {
            Player.Instance.TakeDamage(0.3f);
        }

    }
    public override void Exit()
    {
        base.Exit();
        _bossEnemy?.laserPrefab.gameObject.SetActive(false);
    }
}