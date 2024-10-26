using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class EnemyTurretTrackPlayerState : EnemyState
    {
        public EnemyTurretTrackPlayerState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

        public override void Enter()
        {
            base.Enter();
            SetStateChangeCooldown(.2f); // 可選的冷卻時間
        }

        public override void Update()
        {
            base.Update();
            EnemyTurret turretEnemy = EnemyStateMachine.Enemy as EnemyTurret;

            if (turretEnemy != null)
            {
                // 獲取玩家的位置
                Vector3 playerPosition = Player.Instance.transform.position;
                Vector3 turretPosition = turretEnemy.transform.position;

                // 計算從 Turret 到 Player 的方向並旋轉 Turret
                Vector3 directionToPlayer = playerPosition - turretPosition;
                if (directionToPlayer != Vector3.zero) // 確保不會出現 NaN
                {
                    turretEnemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
                    //Debug.Log("Enemy Turret changed rotation in trackplayerstate");
                }

                // 切換到射擊狀態
                EnemyStateMachine.ChangeState(turretEnemy.ShootState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
