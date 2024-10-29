using System.Collections;
using UnityEngine;

public class SpiralSkillHandler : MonoBehaviour
{
    private Transform VisualSupport;
    public void Initialize(
        Transform visualSupportPrefab, 
        Vector3 position, 
        float damagePerHit, 
        float damageInterval, 
        float skillDuration, 
        float speed, 
        LayerMask enemyLayer)
    {
        VisualSupport = Instantiate(visualSupportPrefab, position, Quaternion.identity);
        StartCoroutine(ApplyDamageOverTime(position, damagePerHit, damageInterval, skillDuration, enemyLayer));
        StartCoroutine(ApplyDisplacementOverTime(position, speed, enemyLayer));
    }
    private IEnumerator ApplyDisplacementOverTime(
        Vector3 position,
        float speed,
        LayerMask enemyLayer)
    {
        while (true)
        {
            // Detect enemies within the area
            Collider[] hitColliders = Physics.OverlapSphere(position, VisualSupport.localScale.x, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Vector3 enemyPos = enemy.transform.position;
                    enemy.transform.position = Vector3.MoveTowards(enemyPos, position, speed * Time.deltaTime);
                }
            }

            yield return null;
        }   
    }
    private IEnumerator ApplyDamageOverTime(
        Vector3 position, 
        float damagePerHit, 
        float damageInterval, 
        float skillDuration, 
        LayerMask enemyLayer)
    {
        float elapsedTime = 0f;

        // Apply damage over time
        while (elapsedTime < skillDuration)
        {
            Debug.Log("elapsed time: " + elapsedTime);
            // Detect enemies within the area
            Collider[] hitColliders = Physics.OverlapSphere(position, VisualSupport.localScale.x, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.GetComponent<Enemy>()?.TakeDamage(damagePerHit);
            }

            // Update the elapsed time
            elapsedTime += damageInterval;

            // Wait for the next damage tick
            yield return new WaitForSeconds(damageInterval);
        }

        // After 5 seconds, the skill ends, and the VisualSupport is destroyed
        Destroy(VisualSupport.gameObject);
        Destroy(gameObject);
    }
}
