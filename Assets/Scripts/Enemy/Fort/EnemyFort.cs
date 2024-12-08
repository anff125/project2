using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFort : Enemy
{
    #region States

    public EnemyFortShootState ShootState { get; private set; }
    public LayerMask playerLayerMask;
    public LayerMask enemyLayerMask;
    [SerializeField] private bool isActivated = false;
    public bool IsActivated => isActivated;
    

    #endregion

    public void Activate()
    {
        StartCoroutine(DelayStart());
    }
    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1f);
        isActivated = true;
    }

    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        ShootState = new EnemyFortShootState(EnemyStateMachine);
        SetIsAlliedWithPlayer(false);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(ShootState);
        WeaponIndex = 2;

        Debug.Log("My layer: " + this.gameObject.layer);
    }

    public override void TakeDamage(IDamageable.Damage damage)
    {
        base.TakeDamage(damage);

        ElementType elementType = damage.ElementType;

        if (elementType == ElementType.Electric)
        {
            TurnToPlayerSide();
        }
    }

    public void TurnToPlayerSide()
    {
        SetIsAlliedWithPlayer(true);
        gameObject.layer = 7; // player
        // Update appearance, behavior, or AI logic here
        Debug.Log("Turn To Player Side");
    }
}
