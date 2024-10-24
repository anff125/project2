using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : Enemy
{
    #region States

    //make track player state and shoot state
    public EnemyShotgunTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyShotgunShootState ShootState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyShotgunTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyShotgunShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 1;
    }

}