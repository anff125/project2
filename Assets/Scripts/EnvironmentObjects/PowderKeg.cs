using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowderKeg : MonoBehaviour, IDamageable
{
    private Coroutine explodeCoroutine = null;
    [SerializeField] private ParticleSystem explosionParticleSystem;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform damageVisualSupport;

    public void TakeDamage(IDamageable.Damage damage)
    {
        ElementType elementType = damage.ElementType;
        if (elementType == ElementType.Ice)
        {
            UnExplode();
        }
        else if (elementType == ElementType.Fire)
        {
            Explode();
        }
    }

    private void Start()
    {
        explosionParticleSystem.Stop();
    }

    public void Ignite(float countdown = .5f)
    {
        Explode(countdown);
    }

    private void UnExplode()
    {
        if (explodeCoroutine != null)
        {
            explosionParticleSystem.Stop();
            Debug.Log("UnExplode");
            damageVisualSupport.gameObject.SetActive(false);

            StopCoroutine(explodeCoroutine);
            visual.gameObject.SetActive(true);

            explodeCoroutine = null;
        }
    }

    private void Explode(float countdown = .5f)
    {
        if (explodeCoroutine == null)
        {
            Debug.Log("Explode");
            damageVisualSupport.gameObject.SetActive(true);
            explodeCoroutine = StartCoroutine(ExplodeCoroutine(countdown));
        }
    }

    private IEnumerator ExplodeCoroutine(float countdown = .5f)
    {
        float blinkInterval = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < countdown)
        {
            visual.gameObject.SetActive(!visual.gameObject.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        visual.gameObject.SetActive(false);
        explosionParticleSystem.Play();
        yield return new WaitForSeconds(0.2f);
        //Deal Fire Damage to nearby entities
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var colliderTMP in colliders)
        {
            if (colliderTMP.transform.position.y > 3f) continue;

            IDamageable damageable = colliderTMP.GetComponent<IDamageable>();
            if (damageable != null)
            {
                IDamageable.Damage damage = new IDamageable.Damage(10, ElementType.Fire, transform);
                damageable.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
        explodeCoroutine = null;
    }

    private void OnEnable()
    {
        explosionParticleSystem.Stop();
    }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
}