using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    public EnemyTurretTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyTurretShootState ShootState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyTurretTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyTurretShootState(EnemyStateMachine);
    }

    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);  // 一開始設置為追蹤狀態
        WeaponIndex = 2;  // 設定武器索引
    }
}
