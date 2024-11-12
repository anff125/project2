using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNeedRecharging : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxCharge;
    [SerializeField] private float currentCharge;
    [SerializeField] private Transform boss;
    [SerializeField] private Transform bulletPrefab;

    private void Start()
    {
        StartCoroutine(ShootingCouroutine());
    }

    private IEnumerator ShootingCouroutine()
    {
        while (true)
        {
            if (currentCharge > 0)
            {
                // Calculate the direction towards the boss and shoot
                if (boss == null)
                {
                    Destroy(gameObject);
                    break;
                }
                Vector3 directionToBoss = (boss.position - transform.position).normalized;
                Transform bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetBulletProperty(directionToBoss, speed: 15, destroyTime: 10f, damage: 10);

                // Optionally, reduce current charge with each shot
                currentCharge -= 5f;
                OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
                {
                    frozenProgressNormalized = currentCharge / maxCharge
                });
            }
            yield return new WaitForSeconds(1f);
        }
    }


    public void TakeDamage(IDamageable.Damage damage)
    {
        if (damage.ElementType == ElementType.Electric)
        {
            currentCharge += damage.Amount;
            if (currentCharge >= maxCharge)
            {
                currentCharge = maxCharge;
            }
            OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
            {
                frozenProgressNormalized = currentCharge / maxCharge
            });
        }
    }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
}