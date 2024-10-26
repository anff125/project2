using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2MeleeAttackState : EnemyState
{
    private float swingDuration = 0.15f; // Time to complete the main swing
    private float recoveryDuration = 0.4f; // Time to return to the initial position
    private float parryRecoveryDuration = 0.2f;
    private float startupDuration = 0.2f; // Time for the startup rotation
    private float swingAngle = 180f;  // Max rotation angle of the swing
    private float startupAngle = -15f; // Startup angle before the main swing
    private Vector3 swingAxis = Vector3.down; // Axis for the main swing
    private int attackCount = 0;
    private float elapsedTime = 0f;
    private enum SwingState { None, Startup, MainSwing, Recovery, Parried }
    private SwingState currentSwingState = SwingState.None;
    public EnemyBoss2MeleeAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private Transform _weapon;
    private bool _beenParried;
    private bool _notyetHit;

    private Quaternion initialRotation;
    private Quaternion startupRotation;
    private Quaternion finalRotation;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        _weapon = _bossEnemy.handPoint;
        _beenParried = false;
        _notyetHit = true;

        attackCount++;
        currentSwingState = SwingState.Startup;
        elapsedTime = 0f;

        initialRotation = _weapon.localRotation;
        startupRotation = initialRotation * Quaternion.Euler(0, startupAngle, 0); // Z-axis startup rotation
        finalRotation = startupRotation * Quaternion.AngleAxis(swingAngle, swingAxis); // Main swing rotation

        Debug.Log("Attack Count: " + attackCount);
    }

    public override void Update()
    {
        base.Update();
        if (_bossEnemy.beenParried)
        {
            _beenParried = true;
        }

        if (currentSwingState == SwingState.Startup)
        {
            HandleStartup();
        }
        else if (currentSwingState == SwingState.MainSwing)
        {
            if (_beenParried)
            {
                _beenParried = false;
                _bossEnemy.beenParried = false;
                currentSwingState = SwingState.Parried;
                elapsedTime = 0f;
                Debug.Log("Boss been parried!");
            }
            else
            {
                HandleMainSwing();
            }
        }
        else if (currentSwingState == SwingState.Recovery)
        {
            HandleRecoverySwing();
        }
        else if (currentSwingState == SwingState.Parried)
        {
            HandleRecoveryParried();
        }
        else if (currentSwingState == SwingState.None)
        {
            EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState);
        }
    }

     private void HandleStartup()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / startupDuration);
        _weapon.localRotation = Quaternion.Lerp(initialRotation, startupRotation, t);

        if (t >= 1f)
        {
            // After startup, begin the main swing
            elapsedTime = 0f;
            currentSwingState = SwingState.MainSwing;
        }
    }
    private void HandleMainSwing()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / swingDuration);
        _weapon.localRotation = Quaternion.Lerp(startupRotation, finalRotation, t);

        if (t >= 1f)
        {
            // After main swing, start returning to the initial position
            elapsedTime = 0f;
            currentSwingState = SwingState.Recovery;
        }

        if (_notyetHit)
        {
            Collider[] hitColliders = Physics.OverlapBox(_bossEnemy.melee.position,
                _bossEnemy.melee.localScale / 2, _bossEnemy.melee.rotation);
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("collide: " + hitCollider.name);
                if (hitCollider.CompareTag("Player"))
                {
                    if (Player.Instance.IsParrying == true)
                    {
                        _beenParried = true;
                        Debug.Log("     Boss2 been parried!!!!!");
                        return;
                    }
                    _notyetHit = false;
                    Player.Instance.TakeDamage(100);
                    Debug.Log("Hit player");
                }
            }
        }
        
    }
    private void HandleRecoverySwing()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / recoveryDuration);
        _weapon.localRotation = Quaternion.Lerp(finalRotation, initialRotation, t);

        if (t >= 1f)
        {
            // After returning, reset the state
            currentSwingState = SwingState.None;
        }
    }
    private void HandleRecoveryParried()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / parryRecoveryDuration);
        _weapon.localRotation = Quaternion.Lerp(finalRotation, initialRotation, t);

        if (t >= 1f)
        {
            // After returning, reset the state
            currentSwingState = SwingState.None;
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}
