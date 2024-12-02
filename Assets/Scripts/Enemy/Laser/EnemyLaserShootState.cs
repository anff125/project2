using System.Collections;
using UnityEngine;

public class EnemyLaserShootState : EnemyState
{
    private EnemyLaser _laserEnemy;
    private Transform _laserInstance;
    private float _laserCooldown = 5f; // 發射冷卻時間
    private bool _hasShotLaser = false;

    public EnemyLaserShootState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _laserEnemy = (EnemyLaser)enemyStateMachine.Enemy;
    }

    public override void Enter()
    {
        base.Enter();
        _hasShotLaser = false;
        SetStateChangeCooldown(.3f);
        // 顯示感嘆號，並在延遲後啟動雷射
        EnemyStateMachine.Enemy.exclamationMark.gameObject.SetActive(true);
        EnemyStateMachine.Enemy.StartCoroutine(StartLaserWithDelay(0.3f));
        StopEnemyMovement(); // 停止敵人移動
    }

    public override void Update()
    {
        base.Update();

        if (_hasShotLaser)
        {
            // 更新雷射持續時間，交由 Laser.cs 處理
            if (_laserInstance == null)
            {
                _hasShotLaser = false;
                EnemyStateMachine.ChangeState(_laserEnemy.TrackPlayerState); // 返回追蹤狀態
            }
        }
    }

    private IEnumerator StartLaserWithDelay(float delay)
    {
        // 等待一段時間再啟動雷射
        yield return new WaitForSeconds(delay);
        _laserEnemy.exclamationMark.gameObject.SetActive(false); // 隱藏感嘆號

        // 直接調用 Laser.cs 的邏輯來發射雷射，無需再次實例化
        _laserInstance = Object.Instantiate(_laserEnemy.laserPrefab, _laserEnemy.transform.position, _laserEnemy.transform.rotation);
        _laserInstance.SetParent(null); // 使雷射脫離敵人父物體，防止隨敵人移動
        _hasShotLaser = true;
    }

    public override void Exit()
    {
        base.Exit();

        // 恢復敵人移動
        ResumeEnemyMovement();

        // 確保離開狀態時銷毀雷射
        if (_laserInstance != null)
        {
            Object.Destroy(_laserInstance.gameObject);
            _laserInstance = null;
        }
    }

    private void StopEnemyMovement()
    {
        // 停止敵人移動的邏輯
        if (_laserEnemy.GetComponent<Rigidbody>() != null)
        {
            _laserEnemy.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void ResumeEnemyMovement()
    {
        // 恢復敵人移動
        if (_laserEnemy.GetComponent<Rigidbody>() != null)
        {
            _laserEnemy.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
