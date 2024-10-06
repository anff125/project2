using UnityEngine;

public class Bullet : MonoBehaviour
{
    //set a layer mask to ignore the entity that shoots the bullet
    [SerializeField] private LayerMask shooterLayerMask;
    private Vector3 _movingDirection;
    private float _speed = 10;
    private int _damage = 1;
    private void SetSpeed(float speed)
    {
        _speed = speed;
    }
    private void SetDirection(Vector3 direction)
    {
        //set bullet direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        _movingDirection = direction;
    }
    private void SetDestroyTime(float time)
    {
        Destroy(gameObject, time);
    }
    private void SetDamage(int damage)
    {
        _damage = damage;
    }
    public void SetBulletProperty(Vector3 bulletDirection, float speed = 10, float destroyTime = 0.5f, int damage = 1)
    {
        SetSpeed(speed);
        SetDirection(bulletDirection);
        SetDestroyTime(destroyTime);
        SetDamage(damage);
    }
    private void Update()
    {
        transform.position += _movingDirection * (Time.deltaTime * _speed);
    }
    private void OnTriggerEnter(Collider collision)
    {
        // ignore the entity that shoots the bullet
        if (shooterLayerMask == (shooterLayerMask | (1 << collision.gameObject.layer)))
        {
            return;
        }
        Debug.Log("Bullet Hit: " + collision.name);
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
        }
        //destroy bullet
        Destroy(gameObject);
    }
}