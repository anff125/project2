using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretNeedRecharging : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxCharge;
    [SerializeField] private float currentCharge;
    [SerializeField] private Transform boss;
    [SerializeField] private Transform bulletForPlayer;
    [SerializeField] private Transform bulletForEnemy;
    private void Start()
    {
        Debug.Log("TurretNeedRecharging Start");
        StartCoroutine(ShootingCoroutine());
        currentCharge = maxCharge / 2;
        OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
        {
            frozenProgressNormalized = currentCharge / maxCharge
        });
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            if (currentCharge > maxCharge / 2)
            {
                if (boss == null)
                {
                    Debug.Log("Boss is null");
                    Destroy(gameObject);
                    break;
                }
                
                Vector3 directionToBoss = (boss.position - transform.position).normalized;
                Transform bullet = Instantiate(bulletForPlayer, transform.position + Vector3.up * .3f, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetBulletProperty(directionToBoss, speed: 15, destroyTime: 10f, damage: 10);

                currentCharge -= 5f;
                OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
                {
                    frozenProgressNormalized = currentCharge / maxCharge
                });
            }
            else if (currentCharge < maxCharge / 2)
            {
                Vector3 directionToPlayer = (Player.Instance.transform.position - transform.position).normalized;
                Transform bullet = Instantiate(bulletForEnemy, transform.position + Vector3.up * .3f, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetBulletProperty(directionToPlayer, speed: 15, destroyTime: 10f, damage: 10);

                currentCharge += 5f;
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
        Charge(damage);
    }

    private void Charge(IDamageable.Damage damage)
    {
        if (damage.ElementType == ElementType.Electric)
        {
            if (damage.Source == Player.Instance.transform)
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
            else
            {
                currentCharge -= damage.Amount;
                if (currentCharge <= 0)
                {
                    currentCharge = 0;
                }
                OnFrozenProgressChange?.Invoke(this, new IDamageable.OnFrozenProgressChangedEventArgs
                {
                    frozenProgressNormalized = currentCharge / maxCharge
                });
            }
        }
    }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChange;
    public event EventHandler<IDamageable.OnFrozenProgressChangedEventArgs> OnFrozenProgressChange;
}