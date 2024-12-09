using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2Danmoku2State : EnemyState
{
    public EnemyBoss2Danmoku2State(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private float delay = .3f;

    public override void Enter()
    {
        base.Enter();
        SetStateChangeCooldown(1f);
        
        if(EnemyStateMachine.EnemyBoss2.exclamationMark == null)
        {
            Debug.LogError("Exclamation mark is null");
        }
        EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.SetActive(true);
        EnemyStateMachine.EnemyBoss2.StartCoroutine(FireWithDelay());
        EnemyStateMachine.EnemyBoss2.danmokuCount2++;
    }
    private IEnumerator FireWithDelay()
    {
        // Wait for the delay duration
        float blinkInterval = .1f;
        float elapsed = 0f;
        while (elapsed <= delay)
        {
            EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.SetActive(!EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.SetActive(false);
        
        EnemyStateMachine.EnemyBoss2.bulletEmitters[0].PlayAnimationMultipleTimes(2, 2, out float clipLength);
        SetStateChangeCooldown(clipLength - 0.4f);
        EnemyStateMachine.EnemyBoss2.animator.SetBool("isAttack", true);
    }

    public override void Update()
    {
        base.Update();

        if (EnemyStateMachine.EnemyBoss2 != null){
            if (EnemyStateMachine.EnemyBoss2.InThirdPhase)
            {
                EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.ThirdPhaseTrackPlayerState);
            }
            else if (EnemyStateMachine.EnemyBoss2.InSecondPhase)
            {
                EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.SecondPhaseTrackPlayerState);
            }
            else
            {
                EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.TrackPlayerState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        EnemyStateMachine.EnemyBoss2.animator.SetBool("isAttack", false);

        // foreach (var emitter in EnemyStateMachine.EnemyBoss2.bulletEmitters)
        // {
        //     emitter.SetRotationIdentity();
        // }
    }
}
