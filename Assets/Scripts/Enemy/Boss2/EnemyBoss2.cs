using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBoss2 : Enemy
{
    public List<BulletEmitter> bulletEmitters = new List<BulletEmitter>();
    public List<WeightedState> weightedStatesPhase1 = new List<WeightedState>();
    public List<WeightedState> weightedStatesPhase2 = new List<WeightedState>();
    public Transform fortEnemyPrefab;
    public Transform waveBulletPrefab;
    public Transform handPoint;
    public Transform melee;
    public bool beenParried;
    public int danmokuCount0;
    public int danmokuCount1;
    public int danmokuCount2;
    public int danmokuCount3;
    private bool inSecondPhase = false;
    public bool InSecondPhase => inSecondPhase;

    #region States

    public EnemyBoss2TrackPlayerState TrackPlayerState { get; private set; }
    public EnemyBoss2SecondPhaseTrackPlayerState SecondPhaseTrackPlayerState { get; private set; }
    public EnemyBoss2MeleeAttackState MeleeState { get; private set; }
    public EnemyBoss2DanmooScatterState ScatterState { get; private set; }
    public EnemyBoss2DanmooCircleState CircleState { get; private set; }
    public EnemyBoss2MoveToState MoveToState { get; private set; }
    public EnemyBoss2Danmoku0State Danmoku0State { get; private set; }
    public EnemyBoss2Danmoku1State Danmoku1State { get; private set; }
    public EnemyBoss2Danmoku2State Danmoku2State { get; private set; }
    public EnemyBoss2Danmoku3State Danmoku3State { get; private set; }
    public EnemyBoss2Danmoku5State Danmoku5State { get; private set; }

    public EnemyBoss2DanmokuWaveState DanmokuWaveState { get; private set; }
    public EnemyBoss2SpawnFortState DanmokuSpawnFortState { get; private set; }
    public EnemyBoss2FinalInitState FinalInitState { get; private set; }

    #endregion

    public void TurnToSecondPhase()
    {
        inSecondPhase = true;
    }

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyBoss2TrackPlayerState(EnemyStateMachine);
        SecondPhaseTrackPlayerState = new EnemyBoss2SecondPhaseTrackPlayerState(EnemyStateMachine);
        MeleeState = new EnemyBoss2MeleeAttackState(EnemyStateMachine);
        ScatterState = new EnemyBoss2DanmooScatterState(EnemyStateMachine);
        CircleState = new EnemyBoss2DanmooCircleState(EnemyStateMachine);
        MoveToState = new EnemyBoss2MoveToState(EnemyStateMachine);
        Danmoku0State = new EnemyBoss2Danmoku0State(EnemyStateMachine);
        Danmoku1State = new EnemyBoss2Danmoku1State(EnemyStateMachine);
        Danmoku2State = new EnemyBoss2Danmoku2State(EnemyStateMachine);
        Danmoku3State = new EnemyBoss2Danmoku3State(EnemyStateMachine);
        Danmoku5State = new EnemyBoss2Danmoku5State(EnemyStateMachine);
        DanmokuWaveState = new EnemyBoss2DanmokuWaveState(EnemyStateMachine);
        DanmokuSpawnFortState = new EnemyBoss2SpawnFortState(EnemyStateMachine);
        FinalInitState = new EnemyBoss2FinalInitState(EnemyStateMachine);
        beenParried = false;
        
        // weightedStates.Add(new WeightedState(MeleeState, 5f));
        // weightedStates.Add(new WeightedState(ScatterState, 8f));
        // weightedStates.Add(new WeightedState(CircleState, 7f));
        weightedStatesPhase1.Add(new WeightedState(Danmoku0State, 4f));
        weightedStatesPhase1.Add(new WeightedState(Danmoku1State, 5f));
        weightedStatesPhase1.Add(new WeightedState(Danmoku2State, 5f));
        weightedStatesPhase1.Add(new WeightedState(Danmoku3State, 4f));
        weightedStatesPhase1.Add(new WeightedState(DanmokuWaveState, 5f));
        weightedStatesPhase1.Add(new WeightedState(DanmokuSpawnFortState, 4f));

        weightedStatesPhase2.Add(new WeightedState(Danmoku1State, 5f));
        weightedStatesPhase2.Add(new WeightedState(Danmoku3State, 4f));
        weightedStatesPhase2.Add(new WeightedState(Danmoku5State, 2f));
        weightedStatesPhase2.Add(new WeightedState(DanmokuWaveState, 5f));
        weightedStatesPhase2.Add(new WeightedState(DanmokuSpawnFortState, 4f));
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 0;
        danmokuCount0 = 0;
        danmokuCount1 = 0;
        danmokuCount2 = 0;
        danmokuCount3 = 0;
    }

    protected override void Update()
    {
        if (IsFrozen)
        {
            foreach (var emitter in bulletEmitters)
            {
                if (emitter.InAnimation)
                    emitter.PlayIdleAnimation();
            }
            return;
        }
        EnemyStateMachine.CurrentEnemyState.Update();
    }
}