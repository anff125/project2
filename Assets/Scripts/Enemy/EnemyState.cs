using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine EnemyStateMachine;
    protected EnemyState(EnemyStateMachine enemyStateMachine)
    {
        EnemyStateMachine = enemyStateMachine;
    }
    protected float TimeInCurrentState;
    private float _stateChangeCooldownTimer = 0f; // Cooldown timer
    public bool CanExit => _stateChangeCooldownTimer <= 0;

    public virtual void Enter()
    {
        //Debug.Log("Entered state: " + this);
        TimeInCurrentState = 0f;
    }

    public virtual void Update()
    {
        if (_stateChangeCooldownTimer > 0)
        {
            _stateChangeCooldownTimer -= Time.deltaTime;
        }
        TimeInCurrentState += Time.deltaTime;
    }

    public virtual void Exit() { }

    public virtual void AnimationFinishTrigger() { }
    protected void SetStateChangeCooldown(float duration)
    {
        _stateChangeCooldownTimer = duration;
    }
}