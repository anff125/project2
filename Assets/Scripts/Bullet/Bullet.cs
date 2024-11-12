using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    //set a layer mask to ignore the entity that shoots the bullet
    [SerializeField]
    public LayerMask shooterLayerMask;
    [SerializeField] private Texture textureForPlayer;
    [SerializeField] private Texture textureForEnemy;
    [SerializeField] protected BulletProperty initProperty;

    [System.Serializable]
    protected class BulletProperty
    {
        public Vector3 direction;
        public float speed;
        public float destroyTime;
        public int damage;
    }

    public bool canBeReflected = true;
    public void SetTextureForPlayer()
    {
        GetComponent<Renderer>().material.mainTexture = textureForPlayer;
    }

    protected virtual void Start()
    {
        initProperty.direction = transform.forward;
    }

    protected Vector3 _movingDirection;
    public Vector3 MovingDirection => _movingDirection;

    private float _speed = 10;
    public float Speed => _speed;

    private int _damage = 1;
    public int Damage => _damage;
    public void SetShooterLayerMask(LayerMask layerMask)
    {
        shooterLayerMask = layerMask;
    }
    protected void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetDirection(Vector3 direction)
    {
        //set bullet direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        _movingDirection = direction;
    }
    protected void SetDestroyTime(float time)
    {
        Destroy(gameObject, time);
    }
    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    public void SetBulletProperty(Vector3 bulletDirection, float speed = 10, float destroyTime = 0.5f, int damage = 25)
    {
        SetDestroyTime(destroyTime);
        SetSpeed(speed);
        SetDirection(bulletDirection);
        SetDamage(damage);
    }
    protected virtual void Update()
    {
        //rotate the bullet to face the moving direction
        transform.rotation = Quaternion.LookRotation(_movingDirection);
        //move the bullet
        transform.position += _movingDirection * (Time.deltaTime * _speed);
    }
    protected void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Bullet Hit: " + collision.name);
        // ignore the entity that shoots the bullet
        if (shooterLayerMask == (shooterLayerMask | (1 << collision.gameObject.layer)))
        {
            return;
        }

        //Debug.Log("Bullet Hit: " + collision.name);
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;
        
        IDamageable.Damage damage = new IDamageable.Damage(_damage, ElementType.Physical, transform);
        damageable.TakeDamage(damage);
        
        //destroy bullet
        Destroy(gameObject);
    }

    private Enemy shooter; // Use the generic Enemy class

    public void SetShooter(Enemy enemy)
    {
        shooter = enemy;
    }
    public Enemy GetShooter()
    {
        return shooter;
    }
    public void ReflectBullet()
    {
        if (shooter != null)
        {
            shooter.IncrementReflectedCount(); // Increment count only on the specific shooter
        }
    }


}