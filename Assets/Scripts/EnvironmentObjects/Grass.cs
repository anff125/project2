using System;
using System.Collections;
using UnityEngine;

public class Grass : MonoBehaviour, IDamageable
{
    private Rigidbody playerRigidbody;
    private bool isPlayerOnGrass;

    [Header("Updraft Settings")]
    public float updraftDuration; // Duration of the updraft in seconds
    public float updraftStrength; // Strength of the updraft force
    public float heightThreshold; // Height threshold for the updraft to be active
    public ForceMode forceMode; // Force mode to apply the updraft force

    private Coroutine updraftCoroutine;
    private bool isOnFire;
    private bool spawned = false;

    private void Start()
    {
        playerRigidbody = Player.Instance.GetComponent<Rigidbody>();
    }

    public void TakeDamage(IDamageable.Damage damage)
    {
        if (updraftCoroutine == null && damage.ElementType == ElementType.Fire)
        {
            isOnFire = true;
            updraftCoroutine = StartCoroutine(UpdraftCoroutine());
        }
        else if (!spawned && damage.ElementType == ElementType.Ice)
        {
            if (!isOnFire)
                return;
            spawned = true;
            InstantiateManager.Instance.InstantiateWater(transform);
        }
    }

    private IEnumerator UpdraftCoroutine()
    {
        float timeElapsed = 0f;

        while (timeElapsed < updraftDuration)
        {
            if (isPlayerOnGrass && Player.Instance.transform.position.y < heightThreshold)
            {
                // Apply force every frame for a continuous updraft effect
                Debug.Log("Applying updraft force");
                playerRigidbody.AddForce(Vector3.up * updraftStrength * Time.deltaTime, forceMode);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        updraftCoroutine = null;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is on grass");
            isPlayerOnGrass = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is not on grass");
            isPlayerOnGrass = false;
        }
    }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
}