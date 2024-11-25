using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyElementalBoss : Enemy
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] public List<float> timeSpace; // Time intervals between each bullet shot, customizable in Unity Editor
    [SerializeField] public List<float> secondPhaseTimeSpace; // Time intervals between each bullet shot, customizable in Unity Editor
    public bool isSecondPhase = false;
    public Transform powderKegPrefab;
    public Transform bulletRazorLeafPrefab;

    public int meleeCount;

    #region States

    public EnemyElementalBossTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyElementalBossShootState ShootState { get; private set; }
    public EnemyElementalBossMeleeState MeleeState { get; private set; }
    public EnemyElementalBossMoveToState MoveToState { get; private set; }
    public EnemyElementalBossSecondPhaseBaseState SecondPhaseBaseState { get; private set; }
    public EnemyElementalBossSecondPhaseInitState SecondPhaseInitState { get; private set; }
    public EnemyElementalBossThrowPowderKegState ThrowPowderKegState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyElementalBossTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyElementalBossShootState(EnemyStateMachine);
        MeleeState = new EnemyElementalBossMeleeState(EnemyStateMachine);
        MoveToState = new EnemyElementalBossMoveToState(EnemyStateMachine);
        SecondPhaseBaseState = new EnemyElementalBossSecondPhaseBaseState(EnemyStateMachine);
        SecondPhaseInitState = new EnemyElementalBossSecondPhaseInitState(EnemyStateMachine);
        ThrowPowderKegState = new EnemyElementalBossThrowPowderKegState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 0;
    }
    protected override float CalculateDamage(IDamageable.Damage damage)
    {
        if (isSecondPhase)
        {
            if (damage.Source.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return damage.Amount * 0.1f;
            }
            else
            {
                return damage.Amount * 5;
            }
        }
        else
        {
            return damage.Amount;
        }
    }
}