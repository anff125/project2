using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float duration = 5f;       // 雷射持續時間
    [SerializeField] private float maxLength = 10f;     // 最長長度
    [SerializeField] private float currentLength = 4f;     // 目前長度
    [SerializeField] private float currentdistance = 6f;     // 目前距離
    [SerializeField] private float distancethRate = 1f;     // 距離增長速度
    [SerializeField] private float width = 2f;     // 寬度
    [SerializeField] private float lengthgrowthRate = 2f;     // 長度增長速度
    [SerializeField] private int damagePerSecond = 10;  // 每秒造成的傷害

    private bool _isExpanding = true;

    private void Start()
    {
        Vector3 laserStartPosition = transform.position + transform.forward * currentdistance;
        transform.position = laserStartPosition;
        transform.localScale = new Vector3(1f, currentLength, width);

        // 設置雷射的角度，使其與敵人的朝向一致
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up); // 這裡會保持平行於地面
        // 轉動 X 軸 90 度，使雷射平行於地面
        transform.Rotate(0f, 0f, 90f);  // 繞 Z 軸旋轉 90 度
        StartCoroutine(ExpandLaser());
    }

    private IEnumerator ExpandLaser()
    {
        float timer = 0f;
        _isExpanding = true;
        while (timer < duration)
        {
            // 控制雷射長度位置
            currentLength = Mathf.Min(maxLength, currentLength + lengthgrowthRate * Time.deltaTime);
            transform.localScale = new Vector3(1f, currentLength, width); 
            transform.position += transform.forward * distancethRate * Time.deltaTime;
            // 更新碰撞器的大小
            if (GetComponent<BoxCollider>() != null)
            {
                GetComponent<BoxCollider>().size = new Vector3(1f, currentLength, width);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        _isExpanding = false;
        Destroy(gameObject); // 雷射持續時間結束後銷毀
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isExpanding) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            // 封裝傷害，並使用與 Bullet 相同的方式
            IDamageable.Damage damage = new IDamageable.Damage(damagePerSecond * Time.deltaTime, ElementType.Physical, transform);
            target.TakeDamage(damage);
        }
    }

}
