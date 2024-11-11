using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBoss2 : Enemy
{
    public List<BulletEmitter> bulletEmitters = new List<BulletEmitter>();
    public Transform handPoint;
    public Transform melee;
    public bool beenParried;
    public int danmokuCount0;
    public int danmokuCount1;
    public int danmokuCount2;

    #region States

    public EnemyBoss2TrackPlayerState TrackPlayerState { get; private set; }
    public EnemyBoss2MeleeAttackState MeleeState { get; private set; }
    public EnemyBoss2DanmooScatterState ScatterState { get; private set; }
    public EnemyBoss2DanmooCircleState CircleState { get; private set; }
    public EnemyBoss2MoveToState MoveToState { get; private set; }
    public EnemyBoss2Danmoku0State Danmoku0State { get; private set; }
    public EnemyBoss2Danmoku1State Danmoku1State { get; private set; }
    public EnemyBoss2Danmoku2State Danmoku2State { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyBoss2TrackPlayerState(EnemyStateMachine);
        MeleeState = new EnemyBoss2MeleeAttackState(EnemyStateMachine);
        ScatterState = new EnemyBoss2DanmooScatterState(EnemyStateMachine);
        CircleState = new EnemyBoss2DanmooCircleState(EnemyStateMachine);
        MoveToState = new EnemyBoss2MoveToState(EnemyStateMachine);
        Danmoku0State = new EnemyBoss2Danmoku0State(EnemyStateMachine);
        Danmoku1State = new EnemyBoss2Danmoku1State(EnemyStateMachine);
        Danmoku2State = new EnemyBoss2Danmoku2State(EnemyStateMachine);
        beenParried = false;
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 0;
        danmokuCount0 = 0;
        danmokuCount1 = 0;
        danmokuCount2 = 0;
    }


}