using UnityEngine;

public class EnemyBossSecondPhaseLaserState : EnemyState
{
    public EnemyBossSecondPhaseLaserState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }
    private EnemyBoss _bossEnemy;
    private Transform _playerTransform;
    private readonly float _rotationSpeed = 2f;
    float _laserScale = 0;
    private Collider _laserCollider;
    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        if (_bossEnemy == null)
        {
            Debug.LogError("Enemy is not a boss");
            return;
        }
        EnemyStateMachine.Enemy.transform.position = new Vector3(0, 0, 0);
        _bossEnemy.secondPhaseLaserPrefab.gameObject.SetActive(true);

        _playerTransform = Player.Instance.transform;
        _laserCollider = _bossEnemy.secondPhaseLaserPrefab.GetComponent<CapsuleCollider>();

        _bossEnemy.secondPhaseLaserPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (_playerTransform)
        {
            Vector3 directionToPlayer = _playerTransform.position - _bossEnemy.transform.position;
            directionToPlayer.y = 0;

            float angle = Vector3.Angle(_bossEnemy.transform.forward, directionToPlayer);

            float adjustedRotationSpeed = _rotationSpeed * (1 + angle / 180f);

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            _bossEnemy.transform.rotation = Quaternion.Slerp(_bossEnemy.transform.rotation, targetRotation, adjustedRotationSpeed * Time.deltaTime);
        }

        if (_laserScale <= 6)
        {
            _laserScale += Time.deltaTime * _rotationSpeed;
            _bossEnemy.secondPhaseLaserPrefab.localScale = new Vector3(0.1f * _laserScale, _laserScale, 0.1f * _laserScale);
            _bossEnemy.secondPhaseLaserPrefab.localPosition = new Vector3(0f, 0.1f, _laserScale);
        }
        else
        {
            _bossEnemy.secondPhaseLaserPrefab.gameObject.SetActive(false);
            _laserScale = 0;
            EnemyStateMachine.ChangeState(_bossEnemy.SecondPhaseIdleState);
        }
        //Deal Damage to player if player is in the _bossEnemy laserPrefab's collider
        if (_laserCollider.bounds.Contains(Player.Instance.transform.position))
        {
            IDamageable.Damage damage = new IDamageable.Damage(0.7f, ElementType.Physical, _bossEnemy.transform);
            Player.Instance.TakeDamage(damage);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}