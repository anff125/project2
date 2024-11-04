using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyElementalBoss : Enemy
{
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] public List<float> timeSpace; // Time intervals between each bullet shot, customizable in Unity Editor

    public Transform bulletRazorLeafPrefab;

    public int meleeCount = 0;
    
    #region States
    
    public EnemyElementalBossTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyElementalBossShootState ShootState { get; private set; }
    public EnemyElementalBossMeleeState MeleeState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyElementalBossTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyElementalBossShootState(EnemyStateMachine);
        MeleeState = new EnemyElementalBossMeleeState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 0;
    }

}