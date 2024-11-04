using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoming : Enemy
{
    public EnemyHomingTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyHomingShootState ShootState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyHomingTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyHomingShootState(EnemyStateMachine);
    }

    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);  // 一開始設置為追蹤狀態
        WeaponIndex = 2;  // 設定武器索引
    }
}
