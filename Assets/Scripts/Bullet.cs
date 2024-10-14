using UnityEngine;

public class Bullet : MonoBehaviour
{
    //set a layer mask to ignore the entity that shoots the bullet
    [SerializeField] private LayerMask shooterLayerMask;
    [SerializeField] private Texture textureForPlayer;
    [SerializeField] private Texture textureForEnemy;
    
    public void SetTextureForPlayer()
    {
        GetComponent<Renderer>().material.mainTexture = textureForPlayer;
    }

    
    private Vector3 _movingDirection;
    public Vector3 MovingDirection => _movingDirection;
    
    private float _speed = 10;
    public float Speed => _speed;
    
    private int _damage = 1;
    public int Damage => _damage;
    public void SetShooterLayerMask(LayerMask layerMask)
    {
        shooterLayerMask = layerMask;
    }
    private void SetSpeed(float speed)
    {
        _speed = speed;
    }
    protected void SetDirection(Vector3 direction)
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
    protected void SetDamage(int damage)
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
    protected void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Bullet Hit: " + collision.name);
        // ignore the entity that shoots the bullet
        if (shooterLayerMask == (shooterLayerMask | (1 << collision.gameObject.layer)))
        {
            return;
        }
        Debug.Log("Bullet Hit: " + collision.name);
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;
        damageable.TakeDamage(_damage);
        //destroy bullet
        Destroy(gameObject);
    }
}