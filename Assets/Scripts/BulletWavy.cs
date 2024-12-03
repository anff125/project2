using UnityEngine;

public class BulletWavy : MonoBehaviour
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

    private float _speed = 5;
    public float Speed => _speed;

    private int _damage = 1;
    public int Damage => _damage;

    private float _waveFrequency = 6f; // Frequency of the wave
    private float _waveAmplitude = 0.04f; // Amplitude of the wave
    private float _waveOffset; // Offset to track wave progression

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
        direction.y = 0; // Ensure the direction has no y-component
        _movingDirection = direction.normalized;

        // Calculate the angle for XZ plane
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle, 0); // Adjust to rotate around the Y-axis for XZ movement
    }
    protected void SetDestroyTime(float time)
    {
        Destroy(gameObject, time);
    }
    protected void SetDamage(int damage)
    {
        _damage = damage;
    }
    public void SetBulletWavyProperty(Vector3 bulletDirection, float speed = 10, float destroyTime = 0.5f, int damage = 25)
    {
        SetSpeed(speed);
        SetDirection(bulletDirection);
        SetDestroyTime(destroyTime);
        SetDamage(damage);
    }
    protected virtual void Update()
    {
        // Apply forward movement in the XZ plane toward the player
        Vector3 movement = new Vector3(_movingDirection.x, 0, _movingDirection.z) * (Time.deltaTime * _speed);
        transform.position += movement;

        // Update wave offset
        _waveOffset += Time.deltaTime * _waveFrequency;
        float waveMovement = Mathf.Sin(_waveOffset) * _waveAmplitude;

        // Add wave movement perpendicular to the bullet's direction in the XZ plane
        Vector3 waveDirection = Vector3.Cross(_movingDirection, Vector3.up).normalized; // Use Vector3.up for the Y-axis to stay in the XZ plane
        transform.position += waveDirection * waveMovement;
    }

    protected void OnTriggerEnter(Collider collision)
    {
        // ignore the entity that shoots the bullet
        if (shooterLayerMask == (shooterLayerMask | (1 << collision.gameObject.layer)))
        {
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // 子彈碰到牆壁時銷毀
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;
        damageable.TakeDamage(_damage);
        //destroy bullet
        Destroy(gameObject);
    }

    private Enemy shooter; // Use the generic Enemy class

    public void SetShooter(Enemy enemy)
    {
        shooter = enemy;
    }

    public void ReflectBullet()
    {
        if (shooter != null)
        {
            shooter.IncrementReflectedCount(); // Increment count only on the specific shooter
        }
    }
}
