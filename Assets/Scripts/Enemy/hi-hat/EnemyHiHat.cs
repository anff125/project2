using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHiHat : Enemy
{
    #region States

    //make track player state and shoot state
    public EnemyHiHatTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyHiHatShootState ShootState { get; private set; }

    #endregion
    
    [SerializeField] public List<float> timeSpace; // Time intervals between each bullet shot, customizable in Unity Editor
    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyHiHatTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyHiHatShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 2;
    }
    
}