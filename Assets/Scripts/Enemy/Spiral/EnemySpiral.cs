using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpiral : Enemy
{
    #region States

//make track player state and shoot state
    public EnemySpiralTrackPlayerState TrackPlayerState { get; private set; }
    public EnemySpiralShootState ShootState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemySpiralTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemySpiralShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 3;
    }

}