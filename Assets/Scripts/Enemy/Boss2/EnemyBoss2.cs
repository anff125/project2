using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBoss2 : Enemy
{
    public List<BulletEmitter> bulletEmitters = new List<BulletEmitter>();
    public Transform handPoint;
    public Transform melee;
    public Transform scatterBulletPrefab;
    public bool beenParried;

    #region States

    public EnemyBoss2TrackPlayerState TrackPlayerState { get; private set; }
    public EnemyBoss2MeleeAttackState MeleeState { get; private set; }
    public EnemyBoss2DanmooScatterState ScatterState { get; private set; }
    public EnemyBoss2DanmooCircleState CircleState { get; private set; }
    public EnemyBoss2Danmoku0State Danmoku0State { get; private set; }
    public EnemyBoss2Danmoku1State Danmoku1State { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyBoss2TrackPlayerState(EnemyStateMachine);
        MeleeState = new EnemyBoss2MeleeAttackState(EnemyStateMachine);
        ScatterState = new EnemyBoss2DanmooScatterState(EnemyStateMachine);
        CircleState = new EnemyBoss2DanmooCircleState(EnemyStateMachine);
        Danmoku0State = new EnemyBoss2Danmoku0State(EnemyStateMachine);
        Danmoku1State = new EnemyBoss2Danmoku1State(EnemyStateMachine);
        beenParried = false;
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 0;
    }


}