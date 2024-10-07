using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBoss : Enemy
{
    public Transform laserPrefab;
    public Transform meleePrefab;
    public Transform smashPrefab;

    #region States

    public EnemyBossTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyBossMeleeAttackState MeleeState { get; private set; }
    public EnemyBossLaserAttackState LaserState { get; private set; }
    public EnemyBossSecondPhaseIdleState SecondPhaseIdleState { get; private set; }
    public EnemyBossSecondPhaseLaserState SecondPhaseLaserState { get; private set; }
    public EnemyBossSecondPhaseSmashState SecondPhaseSmashState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyBossTrackPlayerState(EnemyStateMachine);
        MeleeState = new EnemyBossMeleeAttackState(EnemyStateMachine);
        LaserState = new EnemyBossLaserAttackState(EnemyStateMachine);
        SecondPhaseIdleState = new EnemyBossSecondPhaseIdleState(EnemyStateMachine);
        SecondPhaseLaserState = new EnemyBossSecondPhaseLaserState(EnemyStateMachine);
        SecondPhaseSmashState = new EnemyBossSecondPhaseSmashState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 0;
    }


}