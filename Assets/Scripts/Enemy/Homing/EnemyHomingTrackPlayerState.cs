using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class EnemyHomingTrackPlayerState : EnemyState
    {
        public EnemyHomingTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

        public override void Enter()
        {
            base.Enter();
            SetStateChangeCooldown(.2f); // 可選的冷卻時間
        }

        public override void Update()
        {
            base.Update();
            EnemyHoming HomingEnemy = EnemyStateMachine.Enemy as EnemyHoming;

            if (HomingEnemy != null)
            {
                // 獲取玩家的位置
                Vector3 playerPosition = Player.Instance.transform.position;
                Vector3 HomingPosition = HomingEnemy.transform.position;

                // 計算從 Homing 到 Player 的方向並旋轉 Homing
                Vector3 directionToPlayer = playerPosition - HomingPosition;
                if (directionToPlayer != Vector3.zero) // 確保不會出現 NaN
                {
                    HomingEnemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
                    //Debug.Log("Enemy Homing changed rotation in trackplayerstate");
                }

                // 切換到射擊狀態
                EnemyStateMachine.ChangeState(HomingEnemy.ShootState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
