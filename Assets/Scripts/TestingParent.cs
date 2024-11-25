using System.Collections;
using UnityEngine;

public class TestingParent : MonoBehaviour
{
    [SerializeField] protected Transform childPrefab; // Prefab to spawn
    protected Transform _childTransform; // Reference to the spawned child

    protected virtual void Start()
    {
        if (childPrefab != null)
        {
            StartCoroutine(SpawnChild());
        }
        else
        {
            Debug.LogWarning("Child prefab is not assigned!", this);
        }
    }

    // Continuously attempts to spawn a child every second if _childTransform is null
    protected virtual IEnumerator SpawnChild()
    {
        while (true)
        {
            if (_childTransform == null)
            {
                _childTransform = Instantiate(childPrefab, transform.position, Quaternion.identity, transform);
            }
            yield return new WaitForSeconds(3f);
        }
    }
}