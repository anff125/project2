using UnityEngine;
using System;

public class Weakness : MonoBehaviour, IDamageable
{
    // 設置父物件的 EnemyFire 參考
    private EnemyFire enemyFire;

    private void Awake()
    {
        // 嘗試在父物件中找到 EnemyFire 組件
        enemyFire = GetComponentInParent<EnemyFire>();
        
        if (enemyFire == null)
        {
            Debug.LogError("No EnemyFire component found in parent object!");
        }
    }

    // IDamageable 介面中的 TakeDamage 方法
    public void TakeDamage(float damage, ElementType elementType = ElementType.Physical)
    {
        if (enemyFire != null && elementType == ElementType.Fire)
        {   
            Debug.Log("Weakness Attacked");
            
            // 讓父物件 EnemyFire 執行死亡操作
            StartCoroutine(enemyFire.Die());
        }
    }

    // 空的 OnHealthChange 和 OnFrozenProgressChange 事件實作
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange = delegate { };
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange = delegate { };

}
