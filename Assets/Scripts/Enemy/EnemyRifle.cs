using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifle : Enemy
{
    #region States

//make track player state and shoot state
    public EnemyRifleTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyRifleShootState ShootState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyRifleTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyRifleShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 2;
    }

}