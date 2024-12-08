using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2Danmoku1State : EnemyState
{
    public EnemyBoss2Danmoku1State(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    private float delay = .3f;

    public override void Enter()
    {
        Debug.Log("Enter Danmoku1");
        EnemyStateMachine.EnemyBoss2.danmokuCount0 = 0;
        EnemyStateMachine.EnemyBoss2.danmokuCount2 = 0;
        EnemyStateMachine.EnemyBoss2.danmokuCount3 = 0;

        base.Enter();
        SetStateChangeCooldown(1f);

        EnemyStateMachine.EnemyBoss2.exclamationMark.gameObject.SetActive(true);
        EnemyStateMachine.EnemyBoss2.StartCoroutine(FireWithDelay());
        EnemyStateMachine.EnemyBoss2.danmokuCount1++;
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
        
        EnemyStateMachine.EnemyBoss2.bulletEmitters[0].PlayAnimationMultipleTimes(1, 2, out float clipLength0);
        EnemyStateMachine.EnemyBoss2.bulletEmitters[1].PlayAnimationMultipleTimes(1, 2, out float clipLength1);
        SetStateChangeCooldown(clipLength1);
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

        // foreach (var emitter in EnemyStateMachine.EnemyBoss2.bulletEmitters)
        // {
        //     emitter.SetRotationIdentity();
        // }
    }
}
