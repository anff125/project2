using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : Enemy
{
    #region States

// make track player state and shoot state
    public EnemyFireTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyFireShootState ShootState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyFireTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyFireShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 2;
    }

}