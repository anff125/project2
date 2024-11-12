using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : Enemy
{
    #region States

//make track player state and shoot state
    public EnemyLaserTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyLaserShootState ShootState { get; private set; }

    #endregion
// 新增 laserPrefab 欄位
    public Transform laserPrefab;
    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyLaserTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyLaserShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 2;
    }

}