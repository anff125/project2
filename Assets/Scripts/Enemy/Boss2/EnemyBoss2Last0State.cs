using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2Last0State : EnemyState
{
    public EnemyBoss2Last0State(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss2 _bossEnemy;
    public Animator animator; // Reference to the Animator

    private bool isPlaying;

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss2;
        SetStateChangeCooldown(3f);
        isPlaying = true;
        _bossEnemy.SetInvincibility(true);
        _bossEnemy.StartCoroutine(PlayAnimationRoutine());
    }
    private IEnumerator PlayAnimationRoutine()
    {
        // Play the animation without delay
        EnemyStateMachine.EnemyBoss2.bulletEmitters[0].PlayAnimationMultipleTimes(6, 3, out float clipLength);
        SetStateChangeCooldown(clipLength);
        _bossEnemy.animator.SetBool("isAttack", true);

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
            EnemyStateMachine.ChangeState(EnemyStateMachine.EnemyBoss2.Last2State);
        }
    }

    public override void Exit()
    {
        base.Exit();

        _bossEnemy.animator.SetBool("isAttack", false);

        // foreach (var emitter in EnemyStateMachine.EnemyBoss2.bulletEmitters)
        // {
        //     emitter.SetRotationIdentity();
        // }
    }
}
