using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2FinalInitState : EnemyState
{
    public EnemyBoss2FinalInitState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    public Animator animator; // Reference to the Animator

    private bool isPlaying = true;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        SetStateChangeCooldown(1f);
        _bossEnemy.StartCoroutine(PlayAnimationRoutine());
    }
    private IEnumerator PlayAnimationRoutine()
    {
        float clipLength = 18f;
        SetStateChangeCooldown(clipLength);

        // Play the animation without delay
        EnemyStateMachine.EnemyBoss2.bulletEmitters[0].animator.Play("AnimationDanmoku4");
        EnemyStateMachine.EnemyBoss2.bulletEmitters[1].animator.Play("AnimationDanmoku4_2");

        // Wait for the exact duration of the animation before starting the next iteration
        yield return new WaitForSeconds(clipLength);

        isPlaying = false;
    }

    // private float GetAnimationClipLength(string clipName)
    // {
    //     // Get the RuntimeAnimatorController attached to the Animator
    //     RuntimeAnimatorController controller = animator.runtimeAnimatorController;

    //     // Loop through all animation clips to find the one matching the name
    //     foreach (AnimationClip clip in controller.animationClips)
    //     {
    //         if (clip.name == clipName)
    //         {
    //             return clip.length; // Return the clip length
    //         }
    //     }

    //     Debug.LogWarning($"Animation clip '{clipName}' not found in the Animator.");
    //     return 0f;
    // }

    public override void Update()
    {
        base.Update();

        if (!isPlaying)
        {
            EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.SecondPhaseTrackPlayerState);
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
