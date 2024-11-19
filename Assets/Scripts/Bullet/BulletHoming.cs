using UnityEngine;

public class BulletHoming : Bullet
{
    private Transform playerTransform;
    [SerializeField] private float homingSpeed = 5f; // 调整这个数值来改变追踪速度

    private void Start()
    {
        playerTransform = Player.Instance.transform; // 设定玩家为目标
    }

    protected override void Update()
    {
        // 持续朝向玩家
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * (Time.deltaTime * homingSpeed);
        }
    }
    protected new void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        // 输出调试日志
        Debug.Log("Homing hit: " + collision.name);
    }
}
