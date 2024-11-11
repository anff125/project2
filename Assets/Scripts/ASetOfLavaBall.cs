using UnityEngine;

public class LavaBall : MonoBehaviour
{
    private float lifetime = 18f;

    private void Start()
    {
        // 在生成後 18 秒銷毀物件
        Destroy(gameObject, lifetime);
    }

    private void OnDestroy()
    {
        // 從 InstantiateManager 中移除這個 LavaBall
        if (InstantiateManager.Instance != null)
        {
            InstantiateManager.Instance.RemoveDestroyedLavaBall(gameObject);
        }
    }
}