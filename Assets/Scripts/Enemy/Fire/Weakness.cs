using UnityEngine;
using System;

public class Weakness : MonoBehaviour, IDamageable
{
    // 設置父物件的 EnemyFire 參考
    private EnemyFire enemyFire;
    [SerializeField] private GameObject explosionPrefab; // The explosion effect to instantiate on fire attack
    [SerializeField] private float explosionRadius = 50f; // 攻擊範圍半徑
    [SerializeField] private int explosionDamage = 40; // 火屬性傷害
    // [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float attackAngleThreshold = 180f; // 攻擊角度範圍
    private bool hasBeenHit = false;

    private void Awake()
    {
        // 嘗試在父物件中找到 EnemyFire 組件
        enemyFire = GetComponentInParent<EnemyFire>();
        
        if (enemyFire == null)
        {
            Debug.LogError("No EnemyFire component found in parent object!");
        }
    }

    private Vector3 GetFireAttackDirection()
    {
        // 假設攻擊來源位置是從玩家到弱點的方向
        Vector3 attackPosition = Player.Instance.transform.position;
        return (attackPosition - transform.position).normalized;
    }

    // IDamageable 介面中的 TakeDamage 方法
    public void TakeDamage(float damage, ElementType elementType = ElementType.Physical)
    {
        if (hasBeenHit) return;

        else if (enemyFire != null && elementType == ElementType.Fire)
        {   
            // 檢查火屬性攻擊來向
            Vector3 attackDirection = GetFireAttackDirection();  // 攻擊方向
            Vector3 backDirection = -enemyFire.transform.forward; // 反方向，即背後方向

            // 計算攻擊方向和背後方向的夾角
            float angle = Vector3.Angle(attackDirection, backDirection);

            if (angle > attackAngleThreshold)
            {
                enemyFire.TakeDamage(damage, elementType);
                return;
            }

            Debug.Log("Weakness Attacked");

            hasBeenHit = true;
            
            // 讓父物件 EnemyFire 執行死亡操作
            StartCoroutine(enemyFire.Die());

            // 生成爆炸效果
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Explode();
        }
        else if (enemyFire != null){
            // 非火屬性攻擊，呼叫父物件 EnemyFire 的 TakeDamage 方法
            enemyFire.TakeDamage(damage, elementType);
        }
    }

    public void Explode()
    {
        // 檢測在指定範圍內的所有物件
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {   
            // 排除 cube 自己、父物件 EnemyFire 和玩家
            if (nearbyObject.gameObject == gameObject 
                || nearbyObject.gameObject == enemyFire.gameObject 
                || nearbyObject.CompareTag("Player"))
            {
                continue;
            }

            // 對每個可受傷的敵人施加火屬性傷害
            IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(explosionDamage, ElementType.Fire);
                Debug.Log("Bombed:" + damageable);
            }

        }
    }

    // 用於在場景中顯示爆炸範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // 空的 OnHealthChange 和 OnFrozenProgressChange 事件實作
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange = delegate { };
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange = delegate { };

}
